using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.Engine
{
    public class InvalidEngineCylindersException : Exception, IBadRequestException
    {
        private const string message = "Engine cannot have negative amount or zero cylinders";

        public InvalidEngineCylindersException() : base(message)
        {

        }
    }
}
