using InvestFlow.Application.Interfaces;
using InvestFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvestFlow.Infrastructure.Repositories
{
    public class PortfolioPositionRepository : IPortfolioPositionRepository
    {
        private readonly AppDbContext _dbContext;

        public PortfolioPositionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPositionAsync(PortfolioPosition portfolioPosition, CancellationToken cancellationToken)
        {
            await _dbContext.PortfolioPositions.AddAsync(portfolioPosition, cancellationToken);
        }

        public async Task<PortfolioPosition?> GetByCustomerAndInstrumentAsync(Guid customerId, Guid instrumentId, CancellationToken cancellationToken)
        {
            PortfolioPosition? portfolioPosition = await _dbContext.PortfolioPositions.FirstOrDefaultAsync(x => x.CustomerId == customerId && x.InstrumentId == instrumentId, cancellationToken);
            return portfolioPosition;
        }
    }
}
