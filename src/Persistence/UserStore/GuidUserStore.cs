using Domain.UserDomain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace Persistence.UserStore
{
    public class GuidUserStore : UserStore<User, UserRole, ApplicationContext, Guid>
    {
        public GuidUserStore(ApplicationContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
