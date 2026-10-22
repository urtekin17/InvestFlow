using InvestFlow.Application.Commands;
using InvestFlow.Application.Handlers;
using InvestFlow.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InvestFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly PlaceBuyOrderHandler _buyOrderHandler;
        private readonly PlaceSellOrderHandler _sellOrderHandler;
        public OrdersController(PlaceBuyOrderHandler buyOrderHandler, PlaceSellOrderHandler sellOrderHandler)
        {
            _buyOrderHandler = buyOrderHandler;
            _sellOrderHandler = sellOrderHandler;
        }

        [HttpPost("buy")]
        public async Task<IActionResult> BuyOrderAsync(PlaceBuyOrderCommand buyOrderCommand, CancellationToken cancellationToken)
        {
            Order order = await _buyOrderHandler.HandleAsync(buyOrderCommand,cancellationToken);
            return StatusCode(StatusCodes.Status201Created, order);
        }

        [HttpPost("sell")]
        public async Task<IActionResult> SellrderAsync(PlaceSellOrderCommand sellOrderCommand, CancellationToken cancellationToken)
        {
            Order order = await _sellOrderHandler.HandleAsync(sellOrderCommand, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, order);
        }
    }
}
