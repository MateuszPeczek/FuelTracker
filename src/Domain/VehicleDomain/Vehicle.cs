using Common.Interfaces;
using Domain.UserDomain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.VehicleDomain
{
    public class Vehicle : IEntity
    {
        [Column("VahicleID")]
        public long Id { get; set; }
        public Guid Guid{ get; set; }
        [Range(1900, 2099)]
        public int ProductionYear { get; set; }
        [ForeignKey("Engine")]
        public long EngineID { get; set; }
        [ForeignKey("VehicleManufacturer")]
        public long VehicleManufacturerID { get; set; }
        [ForeignKey("VehicleModel")]
        public long VehicleModelID { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public VehicleType Type { get; set; }

        public virtual Engine Engine { get; set; }
        public virtual VehicleManufacturer VehicleManufacturer { get; set; }
        public virtual VehicleModel VehicleModel { get; set; }
        public virtual User User { get; set; }

    }
}
