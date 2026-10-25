using InvestFlow.Domain.Entities;
using InvestFlow.Domain.Enums;

namespace InvestFlow.Application.Validators
{
    public class OrderIdempotencyValidator
    {
        public static bool IsSameOrder(Order existingOrder, Guid customerId, Guid instrumentId, decimal quantity, decimal price, OrderType orderType)
        {
            return existingOrder.CustomerId == customerId &&
                   existingOrder.InstrumentId == instrumentId &&
                   existingOrder.Quantity == quantity &&
                   existingOrder.Price == price &&
                   existingOrder.Type == orderType;
        }
    }
}
