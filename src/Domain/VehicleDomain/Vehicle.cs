using Common.Interfaces;
using Domain.UserDomain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.VehicleDomain
{
    public class Vehicle : IEntity
    {
        [Column("VahicleID")]
        public long Id { get; set; }
        [Range(1900, 2099)]
        public int ProductionYear { get; set; }
        [ForeignKey("Engine")]
        public long EngineID { get; set; }
        [ForeignKey("Manufacturer")]
        public long ManufacturerID { get; set; }
        [ForeignKey("Model")]
        public long ModelID { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public VehicleType Type { get; set; }

        public virtual Engine Engine { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Model Model { get; set; }
        public virtual User User { get; set; }

    }
}
