using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.FuelStatistics
{
    public class InvalidConsumptionReportIdException : Exception, IBadReuestException
    {
        private const string message = "Invalid report id";

        public InvalidConsumptionReportIdException() :base(message)
        {
            
        }
    }
}
