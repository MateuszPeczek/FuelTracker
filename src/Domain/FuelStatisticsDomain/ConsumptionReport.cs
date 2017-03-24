﻿using Common.Interfaces;
using Domain.Common;
using Domain.VehicleDomain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.FuelStatisticsDomain
{
    public class ConsumptionReport : IEntity
    {
        public Guid Id { get; set; }
        [ForeignKey("Vehicle")]
        [Column("Vehicle")]
        public Guid VehicleId { get; set; }
        public float Distance { get; set; }
        public float FuelBurned { get; set; }
        public float FuelEfficiency { get; set; }
        public decimal PricePerUnit { get; set; }
        public Units Units { get; set; }
        public DateTime DateCreated { get; set; }
        [Timestamp]
        public byte[] RowVersion{ get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
