using System.Collections.Generic;
using Domain.VehicleDomain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Common.Interfaces;

namespace Domain.UserDomain
{
    public class User : IdentityUser<long>, IEntity
    {
        public List<Vehicle> Vehicles { get; set; }
    }
}
