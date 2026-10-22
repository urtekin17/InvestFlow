using InvestFlow.Domain.Entities;

namespace InvestFlow.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken);
        Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
