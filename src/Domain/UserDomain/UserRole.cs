using Common.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.UserDomain
{
    public class UserRole : IdentityRole<Guid>, IEntity
    {
    }
}
