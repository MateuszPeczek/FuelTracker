using CustomExceptions.GroupingIntefaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomExceptions.FuelStatistics
{
    public class FuelSummaryNotFoundException : Exception, INotFoundException
    {
        private const string message = "Fuel summary for vehicle with id: {0} not exists";

        public FuelSummaryNotFoundException(Guid vehicleId) : base(string.Format(message, vehicleId))
        {

        }
    }
}
