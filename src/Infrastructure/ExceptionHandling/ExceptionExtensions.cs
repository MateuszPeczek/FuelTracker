using System;
using System.Text;

namespace Infrastructure.ExceptionHandling
{
    public static class ExceptionExtensions
    {
        public static string GetMessageIncludingInnerExceptions(this Exception ex)
        {
            if (ex.InnerException == null)
                return ex.Message;
            else
                return string.Format($"{ex.Message}{Environment.NewLine}{GetMessageIncludingInnerExceptions(ex.InnerException)}");
        }
    }
}
