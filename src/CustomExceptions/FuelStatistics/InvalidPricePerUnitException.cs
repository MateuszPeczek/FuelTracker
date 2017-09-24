using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.FuelStatistics
{
    public class InvalidPricePerUnitException : Exception, IBadRequestException
    {
        private const string message = "Price per unit cannot be less than 0";

        public InvalidPricePerUnitException() : base(message)
        {

        }
    }
}
