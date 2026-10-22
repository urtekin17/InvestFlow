using InvestFlow.Domain.Entities;

namespace InvestFlow.Application.Interfaces
{
    public interface IPortfolioPositionRepository
    {
        Task AddPositionAsync(PortfolioPosition portfolioPosition, CancellationToken cancellationToken);
        Task<PortfolioPosition?> GetByCustomerAndInstrumentAsync(Guid customerId, Guid instrumentId, CancellationToken cancellationToken);
    }
}
