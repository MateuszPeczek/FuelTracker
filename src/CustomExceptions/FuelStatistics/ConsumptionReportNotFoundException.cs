using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.FuelStatistics
{
    public class ConsumptionReportNotFoundException : Exception, INotFoundException
    {
        private const string message = "Report with id: {0} not exists";

        public ConsumptionReportNotFoundException(Guid id) :base(string.Format(message, id))
        {

        }
    }
}
