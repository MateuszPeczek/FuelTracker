using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.VehicleDomain
{
    public class Manufacturer : IEntity
    {
        public Guid Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        public virtual List<ModelName> ModelNames { get; set; } = new List<ModelName>();
    }
}
