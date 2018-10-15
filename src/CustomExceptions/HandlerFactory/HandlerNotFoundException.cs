using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.HandlerFactory
{
    public class HandlerNotFoundException : Exception, IInternalServerErrorException
    {
        private const string message = "Handler {0} not found";

        public HandlerNotFoundException(string handlerName) : base(string.Format(message, handlerName))
        {

        }
    }
}
