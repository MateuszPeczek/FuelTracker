using Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.UserDomain
{
    public class UserRole : IdentityRole<Guid>, IEntity
    {
    }
}
