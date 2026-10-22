using InvestFlow.Domain.Interfaces;

namespace InvestFlow.Domain.Entities
{
    public class Fund : InvestmentInstrument, IPriceable
    {
        public Fund(string symbol, string name, decimal price) : base(symbol, name, price)
        {
            
        }

        public override decimal CalculateTransactionFee(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount has to be positive.", nameof(amount));
            }
            var fee = amount * 0.001m;

            return fee;
        }

        public decimal GetCurrentPrice()
        {
            return Price;
        }
    }
}
