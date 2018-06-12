using System;

namespace Queries.FuelStatisticsQueries
{
    public class FuelSummaryDetails : BaseDetails
    {
        public Guid VehicleId { get; set; }
        public float AverageConsumption { get; set; }
        public long ReportsNumber { get; set; }
        public decimal DistanceDriven { get; set; }
        public decimal FuelBurned { get; set; }
        public decimal MoneySpent { get; set; }
    }
}
