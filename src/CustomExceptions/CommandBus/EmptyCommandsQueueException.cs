using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.CommandBus
{
    public class EmptyCommandsQueueException : Exception, IInternalServerErrorException
    {
        public EmptyCommandsQueueException(string message) : base(message)
        {

        }
    }
}
