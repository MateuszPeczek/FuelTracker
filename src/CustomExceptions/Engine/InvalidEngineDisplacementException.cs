using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.Engine
{
    public class InvalidEngineDisplacementException : Exception, IBadRequestException
    {
        private const string message = "Engine displacement cannot be equal to zero or negative";

        public InvalidEngineDisplacementException() : base(message)
        {

        }
    }
}
