using Common.Interfaces;
using Domain.Common;
using Domain.UserDomain;
using Domain.VehicleDomain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.FuelStatisticsDomain
{
    public class ConsumptionReport : IEntity
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public float Distance { get; set; }
        public float FuelBurned { get; set; }
        public float FuelEfficiency { get; set; }
        public float PricePerUnit { get; set; }
        public Units Units { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastChanged { get; set; }
        [Timestamp]
        public byte[] RowVersion{ get; set; }

        public virtual Vehicle Vehicle { get; set; }
    }
}
