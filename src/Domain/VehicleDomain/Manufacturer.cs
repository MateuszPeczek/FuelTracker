using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.VehicleDomain
{
    public class Manufacturer : IEntity
    {
        public long Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
