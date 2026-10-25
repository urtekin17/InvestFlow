using InvestFlow.Application.Interfaces;
using InvestFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvestFlow.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        public OrderRepository(AppDbContext dbContext, IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = dbContext;
        }
        public async Task AddOrderAsync(Order order, CancellationToken cancellationToken)
        {
            await _dbContext.Orders.AddAsync(order, cancellationToken);
        }

        public async Task<Order?> GetByIdempotencyKeyAsync(Guid idempotencyKey, CancellationToken cancellationToken)
        {
            Order? order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.IdempotencyKey == idempotencyKey, cancellationToken);
            return order;
        }

        public async Task<Order?> GetByIdempotencyKeyFreshAsync(Guid idempotencyKey, CancellationToken cancellationToken)
        {
            await using (AppDbContext newDbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken))
            {
                Order? order = await newDbContext.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.IdempotencyKey == idempotencyKey, cancellationToken);
                return order;
            }
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Order? order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return order;
        }
    }
}
