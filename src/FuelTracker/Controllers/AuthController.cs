using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FuelTracker.ApiModels.AuthorisationApiModels;
using Microsoft.AspNetCore.Identity;
using Domain.UserDomain;
using FuelTracker.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelTracker.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private UserManager<User> _userManager;

        public AuthController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]PostUser model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = new User()
            {
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _appDbContext.Customers.AddAsync(new Customer { IdentityId = userIdentity.Id, Location = model.Location });
            await _appDbContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }
    }
}
