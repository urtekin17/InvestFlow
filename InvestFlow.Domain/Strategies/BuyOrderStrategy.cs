using InvestFlow.Domain.Entities;
using InvestFlow.Domain.Enums;
using InvestFlow.Domain.Interfaces;

namespace InvestFlow.Domain.Strategies
{
    public class BuyOrderStrategy : IOrderStrategy
    {
        public OrderType Type => OrderType.Buy;
        public Order Trade(Customer customer, InvestmentInstrument instrument, PortfolioPosition portfolio, decimal quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity can not be zero or negative", nameof(quantity));
            }

            var totalAmount = instrument.Price * quantity;

            if (totalAmount > customer.CashBalance)
            {
                throw new InvalidOperationException("Insufficient balance for this process");
            }

            if (customer.Id != portfolio.CustomerId || instrument.Id != portfolio.InstrumentId)
            {
                throw new InvalidOperationException("Wrong customer or instrument.");
            }
            Order order = new Order(customer.Id, instrument.Id, OrderType.Buy, quantity, instrument.Price);
            order.Execute();
            portfolio.AddPosition(quantity, instrument.Price);
            customer.Withdraw(totalAmount);

            return order;
        }
    }
}
