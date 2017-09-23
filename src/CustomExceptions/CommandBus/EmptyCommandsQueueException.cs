using System;

namespace CustomExceptions.CommandBus
{
    public class EmptyCommandsQueueException : Exception
    {
        public EmptyCommandsQueueException(string message) : base(message)
        {

        }
    }
}
