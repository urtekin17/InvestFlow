using InvestFlow.Domain.Enums;

namespace InvestFlow.Domain.Interfaces
{
    public interface IOrderStrategyResolver
    {
        IOrderStrategy Resolve(OrderType type);
    }
}
