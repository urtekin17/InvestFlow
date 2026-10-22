namespace InvestFlow.Domain.Entities
{
    public class Bond : InvestmentInstrument
    {
        public Bond(string symbol, string name, decimal price) : base(symbol,name,price)
        {
            
        }

        public override decimal CalculateTransactionFee(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount has to be positive.", nameof(amount));
            }
            var fee =  amount * 0.0005m;

            return fee;
        }
    }
}
