using InvestFlow.Application.Customers.CreateCustomer;
using InvestFlow.Application.Customers.GetCustomerById;
using InvestFlow.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InvestFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CreateCustomerHandler _createCustomerhandler;
        private readonly GetCustomerByIdHandler _getCustomerByIdhandler;

        public CustomersController(CreateCustomerHandler createCustomerhandler, GetCustomerByIdHandler getCustomerByIdhandler)
        {
            _createCustomerhandler = createCustomerhandler;
            _getCustomerByIdhandler = getCustomerByIdhandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            Customer customer = await _createCustomerhandler.HandleAsync(command, cancellationToken);
            return CreatedAtAction(nameof(GetById),new { id=customer.Id}, customer);
            //return StatusCode(StatusCodes.Status201Created, customer);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            GetCustomerByIdQuery query = new GetCustomerByIdQuery(id);
            Customer customer = await _getCustomerByIdhandler.HandleAsync(query, cancellationToken);
            return Ok(customer);
        }


    }
}
