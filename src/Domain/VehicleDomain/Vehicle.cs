﻿using Common.Helpers;
using Common.Interfaces;
using Domain.UserDomain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.VehicleDomain
{
    public class Vehicle : IEntity
    {
        public Guid Id { get; set; }
        
        [Range(1900, 2099)]
        public int? ProductionYear { get; set; }

        public Guid? EngineId { get; set; }
        [ForeignKey("EngineId")]
        public virtual Engine Engine { get; set; }

        public Guid ModelNameId { get; set; }
        [ForeignKey("ModelNameId")]
        public virtual ModelName ModelName { get; set; }

        //public long UserId { get; set; }
        //[ForeignKey("UserId")]
        //public virtual User User { get; set; }

        public VehicleType? VehicleType { get; set; }


    }
}
