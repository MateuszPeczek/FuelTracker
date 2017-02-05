using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.VehicleDomain
{
    public class Vehicle : IEntity
    {
        public long Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int ProductionYear { get; set; }
        public Engine Engine { get; set; }
        public VehicleType Type { get; set; }
        public long UserId { get; set; }

    }
}
