using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.FuelStatistics
{
    public class InvalidConsumptionReportIdException : Exception, IBadRequestException
    {
        private const string message = "Invalid report id";

        public InvalidConsumptionReportIdException() :base(message)
        {
            
        }
    }
}
