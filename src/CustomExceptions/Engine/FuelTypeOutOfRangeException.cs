using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.Engine
{
    public class FuelTypeOutOfRangeException : Exception, IBadRequestException
    {
        private const string message = "Unsupported fuel type";

        public FuelTypeOutOfRangeException() : base(message)
        {

        }
    }
}
