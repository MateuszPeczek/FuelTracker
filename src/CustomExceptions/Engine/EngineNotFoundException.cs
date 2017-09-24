using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.Engine
{
    public class EngineNotFoundException : Exception, INotFoundException
    {
        private const string message = "Engine with id: {0} not exist";

        public EngineNotFoundException(Guid id) : base(string.Format(message, id))
        {

        }
    }
}
