using Domain.VehicleDomain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Queries.FuelStatisticsQueries
{
    public class FuelSummaryDetails
    {
        public Guid Id { get; set; }
        public Vehicle Vehicle { get; set; }
        public float AverageConsumption { get; set; }
        public long ReportsNumber { get; set; }
        public decimal DistanceDriven { get; set; }
        public decimal FuelBurned { get; set; }
        public decimal MoneySpent { get; set; }
    }
}
