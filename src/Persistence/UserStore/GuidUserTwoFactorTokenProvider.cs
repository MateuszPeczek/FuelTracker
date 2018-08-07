using Domain.UserDomain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.UserStore
{
    public class GuidUserTwoFactorTokenProvider : IUserTwoFactorTokenProvider<User>
    {
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<User> manager, User user)
        {
            throw new NotImplementedException();
        }
    }
}
