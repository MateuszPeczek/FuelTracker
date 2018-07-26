using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace FuelTracker.Helpers
{
    public static class HttpContextExtensions
    {
        public static Guid GetCurrentUserId(this HttpContext httpContext)
        {
            var currentUser = httpContext.User;

            if (currentUser == null || !currentUser.Claims.Any())
            {
                throw new Exception("USer not found or empty claims");
            }

            return new Guid(currentUser.Claims.First(c => c.Type == "UserId").Value);
        }
    }
}
