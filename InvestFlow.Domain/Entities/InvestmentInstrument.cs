using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestFlow.Domain.Entities
{
    public abstract class InvestmentInstrument
    {
        public Guid Id { get; private set; }
        public string Symbol { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public DateTime CreatedAt { get; private set; }


        protected InvestmentInstrument(string symbol, string name, decimal price)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                throw new ArgumentException("Symbol cannot be null or empty.", nameof(symbol));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }
            if (price <= 0)
            {
                throw new ArgumentException("Price cannot be smaller than or equal to 0.", nameof(price));
            }

            Id = new Guid();
            Symbol = symbol;
            Name = name;
            Price = price;
            CreatedAt = DateTime.UtcNow;
        }

        public abstract decimal CalculateTransactionFee(decimal amount);
    }
}
