using InvestFlow.Application.Exceptions;
using InvestFlow.Application.Interfaces;
using InvestFlow.Domain.Entities;

namespace InvestFlow.Application.Customers.CreateCustomer
{
    public class CreateCustomerHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> HandleAsync(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customerInSystem = await _customerRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (customerInSystem is not null)
            {
                throw new CustomerAlreadyExistsException(command.Email);
            }
            Customer newCustomer = new Customer(command.FirstName, command.LastName, command.Email);
            await _customerRepository.AddCustomerAsync(newCustomer, cancellationToken);
            return newCustomer;
        }
    }
}
