namespace InvestFlow.Application.Commands
{
    public record PlaceSellOrderCommand(
        Guid IdempotencyKey,
        Guid CustomerId,
        Guid InstrumentId,
        decimal Quantity,
        decimal Price
    );
}
