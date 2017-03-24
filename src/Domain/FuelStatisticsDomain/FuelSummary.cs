using Common.Interfaces;
using Domain.VehicleDomain;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.FuelStatisticsDomain
{
    public class FuelSummary : IEntity
    {
        public Guid Id { get; set; }

        [ForeignKey("VehicleId")]
        public long VehicleId { get; set; }
        public virtual Vehicle Vehicle{ get; set; }

        public float AverageConsumption { get; set; }

        public long ReportsNumber { get; set; }

        public decimal DistanceDriven { get; set; }

        public decimal FuelBurned { get; set; }

        public decimal MoneySpent { get; set; }
    }
}
