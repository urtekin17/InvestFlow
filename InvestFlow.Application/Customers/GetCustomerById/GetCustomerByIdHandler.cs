using InvestFlow.Application.Exceptions;
using InvestFlow.Application.Interfaces;
using InvestFlow.Domain.Entities;

namespace InvestFlow.Application.Customers.GetCustomerById
{
    public class GetCustomerByIdHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> HandleAsync(GetCustomerByIdQuery customerByIdQuery, CancellationToken cancellationToken)
        {
            Customer? customer = await _customerRepository.GetCustomerByIdAsync(customerByIdQuery.Id, cancellationToken);
            return customer is null ? throw new CustomerNotFoundException(customerByIdQuery.Id) : customer;
        }
    }
}
