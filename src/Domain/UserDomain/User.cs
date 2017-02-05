using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.VehicleDomain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Domain.UserDomain
{
    public class User : IdentityUser<long>, IEntity
    {
        public List<Vehicle> Vehicles { get; set; }
    }
}
