using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Factory
{
    public class QueryHandlerFactory : IQueryHandlerFactory
    {
        public object GetHandler(IQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
