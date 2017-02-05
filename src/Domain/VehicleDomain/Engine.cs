using Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.VehicleDomain
{
    public class Engine : IEntity
    {
        [Column("EngineID")]
        public long Id { get; set; }
        [StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }
        public int Power { get; set; }
        public int Torque { get; set; }
        public int Cylinders { get; set; }
        public float Displacement { get; set; }
        public FuelType FuelType { get; set; }
    }
}
