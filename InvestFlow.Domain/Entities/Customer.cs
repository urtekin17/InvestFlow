namespace InvestFlow.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public decimal CashBalance { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public uint Version { get; private set; }
        public Customer(string firstName, string lastName, string email)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
            }
            if (string.IsNullOrWhiteSpace(lastName)) 
            {
                throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }

            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;    
            Email = email;
            CreatedAt = DateTime.UtcNow;
        }

        public void Deposit(decimal amount)
        {
            try
            {
                if (amount <= 0)
                {
                    throw new ArgumentException("Deposit amount must be positive.", nameof(amount));
                }

                CashBalance += amount;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }            
            
        }

        public void Withdraw(decimal amount)
        {
            try
            {
                if (amount <= 0)
                {
                    throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));
                }
                if (amount > CashBalance)
                {
                    throw new InvalidOperationException("Insufficient funds for withdrawal.");
                }
                CashBalance -= amount;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            
        }   


    }
}
