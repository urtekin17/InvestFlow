using InvestFlow.Domain.Enums;
using InvestFlow.Domain.Interfaces;
using InvestFlow.Domain.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestFlow.Domain.Resolvers
{
    public class OrderStrategyResolver : IOrderStrategyResolver
    {
        private readonly IEnumerable<IOrderStrategy> _strategies;

        public OrderStrategyResolver(IEnumerable<IOrderStrategy> strategies)
        {
            _strategies = strategies;
        }
        public IOrderStrategy Resolve(OrderType type)
        {
            return _strategies.Single(x => x.Type == type);
        }
    }
}
