using Common.Interfaces;
using Domain.VehicleDomain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.UserDomain
{
    public class User : IdentityUser<Guid>, IEntity
    {
        public Guid UserSettingsId { get; set; }
        public virtual UserSettings UserSettings { get; set; }
        public virtual List<Vehicle> Vehicles { get; set; }        
    }
}
