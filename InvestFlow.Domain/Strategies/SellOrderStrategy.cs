using InvestFlow.Domain.Entities;
using InvestFlow.Domain.Enums;
using InvestFlow.Domain.Interfaces;

namespace InvestFlow.Domain.Strategies
{
    public class SellOrderStrategy : IOrderStrategy
    {
        public OrderType Type => OrderType.Sell;

        public Order Trade(Customer customer, InvestmentInstrument instrument, PortfolioPosition portfolio, decimal quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity can not be zero or negative.", nameof(quantity));
            }
            if (portfolio.Quantity < quantity)
            {
                throw new InvalidOperationException("Insufficient quantity for the position.");
            }
            if (customer.Id != portfolio.CustomerId || instrument.Id != portfolio.InstrumentId)
            {
                throw new InvalidOperationException("Wrong customer or instrument.");
            }

            Order order = new Order(new Guid(), customer.Id, instrument.Id, OrderType.Sell, quantity, instrument.Price); 
            order.Execute();
            portfolio.RemovePosition(quantity);
            customer.Deposit(quantity * instrument.Price);
            return order;
        }
    }
}
