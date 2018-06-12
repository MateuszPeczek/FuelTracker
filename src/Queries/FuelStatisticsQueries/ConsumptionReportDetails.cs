using Domain.Common;
using System;

namespace Queries.FuelStatisticsQueries
{
    public class ConsumptionReportDetails : BaseDetails
    {
        private dynamic _vehicleId;
        public dynamic VehicleId
        {
            get
            {
                var guid = new Guid((byte[])_vehicleId);
                return guid.ToString();
            }
            set
            {
                _vehicleId = value;
            }
        }

        public float Distance { get; set; }
        public float FuelBurned { get; set; }
        public float FuelEfficiency { get; set; }
        public float PricePerUnit { get; set; }
        public Units Units { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
