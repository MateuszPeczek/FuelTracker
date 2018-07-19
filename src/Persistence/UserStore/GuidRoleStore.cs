using Domain.UserDomain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace Persistence.UserStore
{
    public class GuidRoleStore : RoleStore<UserRole, ApplicationContext, Guid>
    {
        public GuidRoleStore(ApplicationContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
