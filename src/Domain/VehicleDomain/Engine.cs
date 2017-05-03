using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.VehicleDomain
{
    public class Engine : IEntity
    {      
        public Guid Id { get; set; }

        [StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }

        public int? Power { get; set; }

        public int? Torque { get; set; }

        public int? Cylinders { get; set; }

        public float? Displacement { get; set; }

        public FuelType FuelType { get; set; }
        
        public virtual List<Vehicle> Vehicle { get; set; }
    }
}
