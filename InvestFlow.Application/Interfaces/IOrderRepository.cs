using InvestFlow.Domain.Entities;

namespace InvestFlow.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order, CancellationToken cancellationToken);
        Task<Order?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Order?> GetByIdempotencyKeyAsync(Guid idempotencyKey,CancellationToken cancellationToken);
        Task<Order?> GetByIdempotencyKeyFreshAsync(Guid idempotencyKey, CancellationToken cancellationToken);
    }
}
