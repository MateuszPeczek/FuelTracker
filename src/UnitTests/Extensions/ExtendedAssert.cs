using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests.Extensions
{
    internal static class ExtendedAssert
    {
        internal static void NotThrowsAnyExceptions(Action testCode)
        {
            try
            {
                testCode.Invoke();
            }
            catch (Exception ex)
            {
                var exceptionType = ex.GetType();
                Type innerExceptionType = null;
                var message = new StringBuilder();

                if (ex.InnerException != null)
                    innerExceptionType = ex.InnerException.GetType();

                message.Append($"Throws {ex.GetType()}");
                if (innerExceptionType != null)
                    message.Append($" InnerException - {innerExceptionType.GetType()}");

                throw new Exception(message.ToString());
            }
        }
    }
}
