using Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.UserDomain
{
    public class User : IdentityUser<Guid>, IEntity
    {
        public Guid UserSettingsId { get; set; }
        public virtual UserSettings UserSettings { get; set; }
        //public virtual List<Vehicle> Vehicles { get; set; } = new List<Vehicle>(); 
    }
}
