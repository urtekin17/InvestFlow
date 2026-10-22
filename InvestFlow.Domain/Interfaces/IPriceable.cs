using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestFlow.Domain.Interfaces
{
    public interface IPriceable
    {
        decimal GetCurrentPrice();
    }
}
