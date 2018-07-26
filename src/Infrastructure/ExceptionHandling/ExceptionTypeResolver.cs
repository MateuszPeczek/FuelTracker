using Common.Enums;
using Common.Interfaces;
using CustomExceptions.GroupingIntefaces;
using System;
using System.Linq;
using System.Reflection;

namespace Infrastructure.ExceptionHandling
{
    public class ExceptionTypeResolver : IExceptionTypeResolver
    {
        public ActionStatus ReturnStatusForException(Exception ex)
        {
            var exceptionTypeInfo = ex.InnerException == null ? ex.GetType().GetTypeInfo() : ex.InnerException.GetType().GetTypeInfo();
            var customExceptionType = exceptionTypeInfo.ImplementedInterfaces.FirstOrDefault(i => i.Namespace.Contains("CustomExceptions"));

            if (customExceptionType == typeof(INotFoundException))
                return ActionStatus.NotFound;
            else if (customExceptionType == typeof(IBadRequestException))
                return ActionStatus.BadRequest;
            else
                return ActionStatus.Failure;
        }
    }
}
