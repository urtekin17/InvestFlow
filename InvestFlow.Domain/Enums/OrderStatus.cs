using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestFlow.Domain.Enums
{
    public enum OrderStatus
    {
        Pending =  1,
        Executed = 2,
        Cancelled = 3,
        Failed = 0
    }
}
