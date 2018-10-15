using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.Engine
{
    public class InvalidEngineIdException : Exception, IBadRequestException
    {
        private const string message = "Invalid engine id";

        public InvalidEngineIdException() : base(message)
        {

        }
    }
}
