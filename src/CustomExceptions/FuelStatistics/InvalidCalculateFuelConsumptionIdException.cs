using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

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
