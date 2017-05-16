using Domain.VehicleDomain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Queries.VehicleQueries
{
    public class VehicleDetails
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public int ProductionYear { get; set; }

        public string EngineName { get; set; }

        public int Power { get; set; }

        public int Torque { get; set; }

        public int Cylinders { get; set; }

        public float Displacement { get; set; }

        public FuelType FuelType { get; set; }
    }
}
