using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IQuerySender
    {
        IQueryResult<T> InvokeQuery<T>(IQuery query);
    }
}
