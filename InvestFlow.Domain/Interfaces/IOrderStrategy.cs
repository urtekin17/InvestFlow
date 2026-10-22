using InvestFlow.Domain.Entities;
using InvestFlow.Domain.Enums;

namespace InvestFlow.Domain.Interfaces
{
    public interface IOrderStrategy
    {
        public Order Trade(Customer customer, InvestmentInstrument instrument, PortfolioPosition portfolio, decimal quantity);
        OrderType Type { get; }
    }
}
