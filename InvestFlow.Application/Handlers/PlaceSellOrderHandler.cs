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
    public class PlaceSellOrderHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPortfolioPositionRepository _portfolioPositionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlaceSellOrderHandler(ICustomerRepository customerRepository, IOrderRepository orderRepository, IPortfolioPositionRepository portfolioPositionRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _portfolioPositionRepository = portfolioPositionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> HandleAsync(PlaceSellOrderCommand sellOrderCommand, CancellationToken cancellationToken)
        {
            if (sellOrderCommand.Quantity <= 0) throw new ArgumentException("Quantity can not be zero or less!", nameof(sellOrderCommand.Quantity));
            if (sellOrderCommand.Price <= 0) throw new ArgumentException("Price can not be zero or less!", nameof(sellOrderCommand.Price));

            Order? existingOrder = await _orderRepository.GetByIdempotencyKeyAsync(sellOrderCommand.IdempotencyKey, cancellationToken);
            if (existingOrder != null)
            {
                if (!OrderIdempotencyValidator.IsSameOrder(existingOrder, sellOrderCommand.CustomerId, sellOrderCommand.InstrumentId, sellOrderCommand.Quantity, sellOrderCommand.Price, OrderType.Sell)) 
                {
                    throw new IdempotencyKeyConflictException();
                }
                return existingOrder;
            }
            Customer? customer = await _customerRepository.GetCustomerByIdAsync(sellOrderCommand.CustomerId, cancellationToken);
            if (customer is null) throw new CustomerNotFoundException(sellOrderCommand.CustomerId);

            Order order = new Order(sellOrderCommand.IdempotencyKey, sellOrderCommand.CustomerId, sellOrderCommand.InstrumentId, OrderType.Sell, sellOrderCommand.Quantity, sellOrderCommand.Price);
            PortfolioPosition? portfolioPosition = await _portfolioPositionRepository.GetByCustomerAndInstrumentAsync(sellOrderCommand.CustomerId,sellOrderCommand.InstrumentId, cancellationToken);
            if(portfolioPosition is null)
            {
                throw new InvalidOperationException("Position not found for Sell operation!");
            }
            order.Execute();
            portfolioPosition.RemovePosition(sellOrderCommand.Quantity);
            decimal totalAmount = sellOrderCommand.Quantity * sellOrderCommand.Price;
            customer.Deposit(totalAmount);

            await _orderRepository.AddOrderAsync(order, cancellationToken);

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is PostgresException pgEx
                      && pgEx.SqlState == PostgresErrorCodes.UniqueViolation
                      && pgEx.ConstraintName == "IX_Orders_IdempotencyKey")
            {
                Order? existOrder = await _orderRepository.GetByIdempotencyKeyFreshAsync(sellOrderCommand.IdempotencyKey, cancellationToken);
                if (existOrder == null) throw;
                if (!OrderIdempotencyValidator.IsSameOrder(existOrder, sellOrderCommand.CustomerId, sellOrderCommand.InstrumentId, sellOrderCommand.Quantity, sellOrderCommand.Price, OrderType.Sell))
                {
                    throw new IdempotencyKeyConflictException();
                }
                return existOrder;
            }

            return order;
        }
    }
}
