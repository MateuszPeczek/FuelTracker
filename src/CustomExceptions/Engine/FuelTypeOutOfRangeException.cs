using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

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
