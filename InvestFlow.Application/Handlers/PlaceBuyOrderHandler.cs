using InvestFlow.Application.Commands;
using InvestFlow.Application.Exceptions;
using InvestFlow.Application.Interfaces;
using InvestFlow.Application.Validators;
using InvestFlow.Domain.Entities;
using InvestFlow.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace InvestFlow.Application.Handlers
{
    public class PlaceBuyOrderHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPortfolioPositionRepository _portfolioPositionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlaceBuyOrderHandler(ICustomerRepository customerRepository, IOrderRepository orderRepository, IPortfolioPositionRepository portfolioPositionRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _portfolioPositionRepository = portfolioPositionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> HandleAsync(PlaceBuyOrderCommand buyOrderCommand, CancellationToken cancellationToken)
        {
            if(buyOrderCommand.Quantity <= 0) throw new ArgumentException("Quantity can not be zero or less!", nameof(buyOrderCommand.Quantity));
            if (buyOrderCommand.Price <= 0) throw new ArgumentException("Price can not be zero or less!", nameof(buyOrderCommand.Price));

            Customer? customer = await _customerRepository.GetCustomerByIdAsync(buyOrderCommand.CustomerId, cancellationToken);
            if (customer == null) throw new CustomerNotFoundException(buyOrderCommand.CustomerId);

            decimal totalPrice = buyOrderCommand.Quantity * buyOrderCommand.Price;
            Order? existingOrder = await _orderRepository.GetByIdempotencyKeyAsync(buyOrderCommand.IdempotencyKey,cancellationToken);
            if(existingOrder != null)
            {
                if (!OrderIdempotencyValidator.IsSameOrder(existingOrder, buyOrderCommand.CustomerId, buyOrderCommand.InstrumentId, buyOrderCommand.Quantity, buyOrderCommand.Price, OrderType.Buy))
                {
                    throw new IdempotencyKeyConflictException();
                }
                return existingOrder;
            }
            
            Order order = new Order(buyOrderCommand.IdempotencyKey, buyOrderCommand.CustomerId, buyOrderCommand.InstrumentId, OrderType.Buy, buyOrderCommand.Quantity, buyOrderCommand.Price);
            
            PortfolioPosition? portfolioPosition = await _portfolioPositionRepository.GetByCustomerAndInstrumentAsync(buyOrderCommand.CustomerId, buyOrderCommand.InstrumentId, cancellationToken);

            if (portfolioPosition is null)
            {
                portfolioPosition = new PortfolioPosition(buyOrderCommand.CustomerId, buyOrderCommand.InstrumentId);
                await _portfolioPositionRepository.AddPositionAsync(portfolioPosition, cancellationToken);
            }
            portfolioPosition.AddPosition(buyOrderCommand.Quantity, buyOrderCommand.Price);
            
            order.Execute();
            customer.Withdraw(totalPrice);
            await _orderRepository.AddOrderAsync(order,cancellationToken);

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is PostgresException pgEx
                      && pgEx.SqlState == PostgresErrorCodes.UniqueViolation
                      && pgEx.ConstraintName == "IX_Orders_IdempotencyKey")
            {
                Order? existOrder = await _orderRepository.GetByIdempotencyKeyFreshAsync(buyOrderCommand.IdempotencyKey, cancellationToken);
                if (existOrder == null) throw;
                if (!OrderIdempotencyValidator.IsSameOrder(existOrder, buyOrderCommand.CustomerId, buyOrderCommand.InstrumentId, buyOrderCommand.Quantity, buyOrderCommand.Price, OrderType.Buy))
                {
                    throw new IdempotencyKeyConflictException();
                }
                return existOrder;
            }

            return order;

        }
    }
}
