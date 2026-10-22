namespace InvestFlow.Application.Exceptions
{
    public class CustomerAlreadyExistsException : Exception
    {
        public CustomerAlreadyExistsException(string email) : base($"Customer with email '{email}' already exists.")
        {
        }
    }
}
