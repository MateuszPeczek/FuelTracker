using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface IQueryResult<T>
    {
        T Data { get; set; }
        string ExceptionMessage { get; set; }
    }
}
