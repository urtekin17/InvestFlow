using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestFlow.Application.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(Guid id) : base($"The customer that is searched by id ({id}) is not found.")
        {

        }
    }
}
