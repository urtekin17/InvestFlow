using InvestFlow.Infrastructure;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("InvestFlow Console started!");

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseNpgsql("Host=localhost;Port=5432;Database=investflowdb;Username=postgres")
    .Options;

using var contextA = new AppDbContext(options);
using var contextB = new AppDbContext(options);

var positionA = contextA.PortfolioPositions.FirstOrDefault(x => x.Id == Guid.Parse("9015e5c2-7485-4250-bc0d-68ea5895be99"));
var positionB = contextB.PortfolioPositions.FirstOrDefault(x => x.Id == Guid.Parse("9015e5c2-7485-4250-bc0d-68ea5895be99"));
var positionList = contextA.PortfolioPositions.ToList();
Console.WriteLine($"Customer A Version: {positionA.Version}");
Console.WriteLine($"Customer B Version: {positionB.Version}");

positionA.AddPosition(3,120);
contextA.SaveChanges();

try
{
    positionB.RemovePosition(2);
    contextB.SaveChanges();
}
catch (DbUpdateConcurrencyException ex)
{
    Console.WriteLine(ex.Message);
	throw;
}
