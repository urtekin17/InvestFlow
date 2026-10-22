using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestFlow.Application.Customers.CreateCustomer
{
    public record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string Email
);
}
