using Domain.UserDomain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Persistence.UserStore
{
    public class GuidUserRoleManager : RoleManager<UserRole>
    {
        public GuidUserRoleManager(IRoleStore<UserRole> store, IEnumerable<IRoleValidator<UserRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<UserRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}
