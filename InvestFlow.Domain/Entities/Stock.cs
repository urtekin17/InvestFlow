using InvestFlow.Domain.Interfaces;

namespace InvestFlow.Domain.Entities
{
    public class Stock : InvestmentInstrument, IPriceable
    {
        public Stock(string symbol, string name, decimal price) :base(symbol, name, price)
        {
            
        }

        public override decimal CalculateTransactionFee(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount has to be positive.", nameof(amount));
            }
            var fee = amount * 0.002m;

            return fee;
        }

        public decimal GetCurrentPrice()
        {
            return Price;
        }
    }
}
