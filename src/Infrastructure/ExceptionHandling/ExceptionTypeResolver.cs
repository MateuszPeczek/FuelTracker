using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Common.Enums;
using System.Reflection;
using System.Linq;
using CustomExceptions.GroupingIntefaces;

namespace Infrastructure.ExceptionHandling
{
    public class ExceptionTypeResolver : IExceptionTypeResolver
    {
        public ActionStatus ReturnCommandStatusForException(Exception ex)
        {
            var exceptionTypeInfo = ex.InnerException == null ? ex.GetType().GetTypeInfo() : ex.InnerException.GetType().GetTypeInfo();
            var customExceptionType = exceptionTypeInfo.ImplementedInterfaces.Where(i => i.Namespace.Contains("CustomExceptions")).FirstOrDefault();

            if (customExceptionType == typeof(INotFoundException))
                return ActionStatus.NotFound;

            if (customExceptionType == typeof(IBadRequestException))
                return ActionStatus.BadRequest;

                return ActionStatus.Failure;
        }
    }
}
