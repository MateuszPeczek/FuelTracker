using Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.VehicleDomain
{
    public class Manufacturer : IEntity
    {
        [Column("ManufacturerID")]
        public long Id { get; set; }
        [StringLength(20)]
        public string Name { get; set; }
    }
}
