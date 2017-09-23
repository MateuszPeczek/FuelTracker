using System;

namespace CustomExceptions.HandlerFactory
{
    public class HandlerNotFoundException : Exception
    {
        public HandlerNotFoundException(string message) : base(message)
        {

        }
    }
}
