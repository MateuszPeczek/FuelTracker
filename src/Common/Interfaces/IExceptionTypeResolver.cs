using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface IExceptionTypeResolver
    {
        ActionStatus ReturnCommandStatusForException(Exception ex);
    }
}
