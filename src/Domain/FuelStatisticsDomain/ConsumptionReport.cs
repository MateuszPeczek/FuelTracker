using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.FuelStatisticsDomain
{
    public class ConsumptionReport : IEntity
    {
        public long Id { get; set; }
        public long VehicleId { get; set; }
        public float Distance { get; set; }
        public float FuelBurned { get; set; }
        public float FuelEfficiency { get; set; }
        public decimal PricePerUnit { get; set; }
        public Units Units { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
