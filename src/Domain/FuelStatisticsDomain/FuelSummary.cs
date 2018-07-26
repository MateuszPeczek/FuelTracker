using Common.Interfaces;
using Domain.Common;
using Domain.VehicleDomain;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.FuelStatisticsDomain
{
    public class FuelSummary : IEntity
    {
        public Guid Id { get; set; }

        public Guid VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle{ get; set; }

        public float AverageConsumption { get; set; }

        public long ReportsNumber { get; set; }

        public float DistanceDriven { get; set; }

        public float FuelBurned { get; set; }

        public float MoneySpent { get; set; }
        public Units Units { get; set; }
    }
}
