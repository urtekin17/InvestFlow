namespace InvestFlow.Domain.Entities
{
    public class PortfolioPosition
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid InstrumentId { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal AverageCost { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public uint Version { get; private set; }
        public PortfolioPosition(Guid customerId, Guid instrumentId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            InstrumentId = instrumentId;
            Quantity = 0;
            AverageCost = 0;
            CreatedAt = DateTime.UtcNow;
        }

        public void AddPosition(decimal quantity, decimal price)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity can not be zero or negative.", nameof(quantity));
            }
            if (price <= 0)
            {
                throw new ArgumentException("Price can not be zero or negative.", nameof(price));
            }

            var oldAmount = Quantity * AverageCost;
            var newAmount = quantity * price;

            var totalAmount = oldAmount + newAmount;
            var totalQuantity = Quantity + quantity;

            var newAvgCost = totalAmount / totalQuantity;
            Quantity += quantity;
            AverageCost = newAvgCost;
            UpdatedAt = DateTime.UtcNow;

        }

        public void RemovePosition(decimal quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity can not be zero or negative.", nameof(quantity));
            }
            if (Quantity < quantity)
            {
                throw new InvalidOperationException("Insufficient total quantity!");
            }
            Quantity -= quantity;
            UpdatedAt = DateTime.UtcNow;
            if (Quantity == 0)
            {
                AverageCost = 0;
            }
        }
    }
}
