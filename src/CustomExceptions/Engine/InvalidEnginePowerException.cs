using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.Engine
{
    public class InvalidEnginePowerException : Exception, IBadRequestException
    {
        private const string message = "Engine cannot have negative amount of power";

        public InvalidEnginePowerException() : base(message)
        {
                
        }
    }
}
