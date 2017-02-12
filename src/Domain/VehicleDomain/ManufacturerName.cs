using Common.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.VehicleDomain
{
    public class Manufacturer : IEntity
    {
        public long Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        public virtual List<ModelName> ModelName { get; set; }
    }
}
