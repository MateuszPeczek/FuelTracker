using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.CommunicationModels
{
    public class QueryResult<T> : IQueryResult<T>
    {
        public T Data { get; set; }
        public Exception Exception { get; set; }
    }
}
