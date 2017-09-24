using CustomExceptions.GroupingIntefaces;
using System;

namespace CustomExceptions.FuelStatistics
{
    public class ConsumptionReportNotFoundException : Exception, INotFoundException
    {
        private const string message = "Report with id: {1} not exists for vehicleId {0}";

        public ConsumptionReportNotFoundException(Guid vehicleId, Guid fuelReportId) :base(string.Format(message, vehicleId, fuelReportId))
        {

        }
    }
}
