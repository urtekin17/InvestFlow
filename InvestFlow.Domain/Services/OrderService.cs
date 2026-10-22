using InvestFlow.Domain.Entities;
using InvestFlow.Domain.Enums;
using InvestFlow.Domain.Interfaces;

namespace InvestFlow.Domain.Services
{
    public class OrderService
    {
        private readonly IOrderStrategyResolver _orderStrategyResolver;

        public OrderService(IOrderStrategyResolver orderStrategyResolver)
        {
            _orderStrategyResolver = orderStrategyResolver;
        }

        public Order Trade(Customer customer, InvestmentInstrument instrument, PortfolioPosition portfolio, OrderType type, decimal quantity)
        {
            var strategy = _orderStrategyResolver.Resolve(type);
            return strategy.Trade(customer,instrument,portfolio,quantity);
        }
    }
}
