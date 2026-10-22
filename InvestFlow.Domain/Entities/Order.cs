using InvestFlow.Domain.Enums;

namespace InvestFlow.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid InstrumentId { get; private set; }

        public OrderType Type { get; private set; }
        public OrderStatus Status { get; private set; }

        public decimal Quantity { get; private set; }
        public decimal Price { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? ExecutedAt { get; private set; }

        public decimal TotalAmount => Quantity * Price;

        public Guid IdempotencyKey { get; private set; }

        public Order(Guid idempotencyKey, Guid customerId, Guid instrumentId, OrderType  type, decimal quantity, decimal price)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity has to be positive.",nameof(quantity));
            }
            if (price <= 0)
            {
                throw new ArgumentException("Price can not be zero or negative.",nameof(price));
            }

            if (idempotencyKey == Guid.Empty) throw new ArgumentException("Idempotency key can not be empty!",nameof(idempotencyKey));
            IdempotencyKey = idempotencyKey; 
            Id = Guid.NewGuid();
            CustomerId = customerId;
            InstrumentId = instrumentId;
            Type = type;
            Status = OrderStatus.Pending;
            Quantity = quantity;
            Price = price;
            CreatedAt = DateTime.UtcNow;
        }

        public void Execute()
        {
            if (Status == OrderStatus.Pending)
            {
                Status = OrderStatus.Executed;
                ExecutedAt = DateTime.UtcNow;
            }
            else
            {
                throw new InvalidOperationException($"Order in status of {Status}. Can not be executed!");
            }
        }
    }
}
