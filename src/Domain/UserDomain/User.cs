using System.Collections.Generic;
using Domain.VehicleDomain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Common.Interfaces;
using System;

namespace Domain.UserDomain
{
    public class User : IdentityUser<Guid>, IEntity
    {
        public Guid UserSettingsId { get; set; }
        public virtual UserSettings UserSettings { get; set; }
        public virtual List<Vehicle> Vehicles { get; set; }        
    }
}
