using InvestFlow.Application.Commands;
using InvestFlow.Application.Exceptions;
using InvestFlow.Application.Interfaces;
using InvestFlow.Domain.Entities;
using InvestFlow.Domain.Enums;

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

            Customer? customer = await _customerRepository.GetCustomerByIdAsync(sellOrderCommand.CustomerId, cancellationToken);
            if (customer is null) throw new CustomerNotFoundException(sellOrderCommand.CustomerId);

            PortfolioPosition? portfolioPosition = await _portfolioPositionRepository.GetByCustomerAndInstrumentAsync(sellOrderCommand.CustomerId,sellOrderCommand.InstrumentId, cancellationToken);
            if(portfolioPosition is null)
            {
                throw new InvalidOperationException("Position not found for Sell operation!");
            }
            
            Order order = new Order(sellOrderCommand.IdempotencyKey, sellOrderCommand.CustomerId, sellOrderCommand.InstrumentId, OrderType.Sell, sellOrderCommand.Quantity, sellOrderCommand.Price);
            order.Execute();
            portfolioPosition.RemovePosition(sellOrderCommand.Quantity);
            decimal totalAmount = sellOrderCommand.Quantity * sellOrderCommand.Price;
            customer.Deposit(totalAmount);

            await _orderRepository.AddOrderAsync(order, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return order;
        }
    }
}
