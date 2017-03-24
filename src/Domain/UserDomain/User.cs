using System.Collections.Generic;
using Domain.VehicleDomain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Common.Interfaces;
using System;

namespace Domain.UserDomain
{
    public class User : IdentityUser<Guid>, IEntity
    {
        //public virtual List<Vehicle> Vehicles { get; set; }
    }
}
