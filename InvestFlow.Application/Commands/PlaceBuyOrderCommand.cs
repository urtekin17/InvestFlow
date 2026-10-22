using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestFlow.Application.Commands
{
    public record PlaceBuyOrderCommand(
        Guid IdempotencyKey,
        Guid CustomerId,
        Guid InstrumentId,
        decimal Quantity,
        decimal Price
    );
}
