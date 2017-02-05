﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.VehicleDomain
{
    public class Engine : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Power { get; set; }
        public int Torque { get; set; }
        public int Cylinders { get; set; }
        public float Displacement { get; set; }
        public FuelType FuelType { get; set; }
    }
}
