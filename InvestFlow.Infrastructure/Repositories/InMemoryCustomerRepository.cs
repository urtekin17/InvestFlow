using InvestFlow.Application.Interfaces;
using InvestFlow.Domain.Entities;

namespace InvestFlow.Infrastructure.Repositories
{
    public class InMemoryCustomerRepository : ICustomerRepository
    {
        private readonly List<Customer> _customers = new();

        public void AddCustomer(Customer customer)
        {
            _customers.Add(customer);    
        }

        public Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Customer? GetByEmail(string email)
        {
            return _customers.FirstOrDefault(e => string.Equals(e.Email, email, StringComparison.OrdinalIgnoreCase));
        }

        public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Customer? GetCustomerById(Guid id)
        {
            return _customers.FirstOrDefault(x => x.Id == id);
        }

        public Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}
