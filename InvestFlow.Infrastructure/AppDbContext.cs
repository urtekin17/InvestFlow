using InvestFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvestFlow.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> contextOptions) : base(contextOptions)
        {
                
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PortfolioPosition> PortfolioPositions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PortfolioPosition>().HasIndex(x => new { x.CustomerId, x.InstrumentId }).IsUnique();
            modelBuilder.Entity<Order>().HasIndex(x => new { x.IdempotencyKey }).IsUnique(); 
            modelBuilder.Entity<Customer>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<PortfolioPosition>().Property(p => p.Version).IsRowVersion();
        }
    }

}
