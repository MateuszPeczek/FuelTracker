using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IQuerySender
    {
        TResult Send<TResult>(IQuery query) where TResult : class;
    }
}
