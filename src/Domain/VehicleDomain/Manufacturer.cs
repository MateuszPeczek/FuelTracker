using Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Domain.VehicleDomain
{
    public class Manufacturer : IEntity
    {
        public long Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
