using CustomExceptions.GroupingIntefaces;
using System;

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
