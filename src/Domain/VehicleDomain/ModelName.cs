using Common.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.VehicleDomain
{
    public class ModelName : IEntity
    {
        public long Id { get; set; }

        public long ManufacturerId { get; set; }
        [ForeignKey("ManufacturerId")]
        public virtual Manufacturer Manufacturer { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        public virtual List<Vehicle> Vehicle { get; set; }

    }
}
