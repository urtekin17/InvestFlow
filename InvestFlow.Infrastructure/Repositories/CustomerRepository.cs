using InvestFlow.Application.Interfaces;
using InvestFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvestFlow.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _dbContext;
        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            await _dbContext.Customers.AddAsync(customer, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            Customer? customer = await _dbContext.Customers.FirstOrDefaultAsync(x => EF.Functions.ILike(x.Email,email),cancellationToken);
            return customer;
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Customer? customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return customer;
        }
    }
}
