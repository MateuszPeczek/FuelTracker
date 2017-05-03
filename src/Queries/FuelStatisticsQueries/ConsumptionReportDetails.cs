using Domain.Common;
using Domain.VehicleDomain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Queries.FuelStatisticsQueries
{
    public class ConsumptionReportDetails
    {
        public Guid Id { get; set; }
        public Vehicle Vehicle { get; set; }
        public float Distance { get; set; }
        public float FuelBurned { get; set; }
        public float FuelEfficiency { get; set; }
        public decimal PricePerUnit { get; set; }
        public Units Units { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
