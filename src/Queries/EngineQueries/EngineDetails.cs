﻿using Domain.VehicleDomain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Queries.EngineQueries
{
    public class EngineDetails 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Power { get; set; }
        public int? Torque { get; set; }
        public int? Cylinders { get; set; }
        public int? Displacement { get; set; }
        public FuelType FuelType { get; set; }
    }
}