using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.FuelStatistics
{
    public class InvalidCalculateFuelConsumptionIdException : Exception, IBadRequestException
    {
        private const string message = "Invalid fuel calculation id";

        public InvalidCalculateFuelConsumptionIdException() :base(message)
        {
            
        }
    }
}
