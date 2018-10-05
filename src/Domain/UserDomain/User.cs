using Common.Interfaces;
using Domain.VehicleDomain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.UserDomain
{
    public class User : IdentityUser<Guid>, IEntity
    {
        [MaxLength(20)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; }

        public virtual List<Vehicle> Vehicles { get; set; }

        public virtual UserSettings UserSettings { get; set; }
    }
}
