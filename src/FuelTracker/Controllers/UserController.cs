using Commands.UserCommands;
using Common.Enums;
using Common.Interfaces;
using FuelTracker.ApiModels.UserApiModels;
using FuelTracker.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Queries.UserQueries;
using System;

namespace FuelTracker.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/users")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly ICommandSender commandBus;
        private readonly IQuerySender queryBus;
        private readonly Common.Interfaces.IAuthorizationService authService;

        public UserController(ICommandSender commandBus, IQuerySender queryBus, Common.Interfaces.IAuthorizationService authService)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
            this.authService = authService;
        }

        private IQueryResult<UserDetails> GetUserDetails(Guid? userId = null)
        {
            IQueryResult<UserDetails> result;

            if (userId.HasValue)
                result = queryBus.InvokeQuery<UserDetails>(new GetSingleUser(userId.Value));
            else
                result = queryBus.InvokeQuery<UserDetails>(new GetSingleUser(HttpContext.GetCurrentUserId()));

            return result;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateUser([FromBody]PostUser model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddUser(model.Email, model.Password);
                commandBus.AddCommand(command);
                commandBus.InvokeCommandsQueue();

                var result = GetUserDetails(command.Id);

                return CreatedAtRoute(
                    "GetUser",
                    new { userEmail = command.Email },
                    result.Data
                    );
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody]PutUser model)
        {
            if (ModelState.IsValid)
            {
                var userData = authService.GetCurrentUserData();


                var command = new UpdateUser(userData.UserId, model.FirstName, model.LastName);
                commandBus.AddCommand(command);
                commandBus.InvokeCommandsQueue();

                var result = GetUserDetails(userData.UserId);

                return CreatedAtRoute(
                    "GetUser",
                    new { firstName = command.FirstName, lastName = command.LastName },
                    result
                    );
            }

            return BadRequest(ModelState);
        }

        //GET: api/users       
        [HttpGet(Name = "GetUser")]
        public IActionResult GetUser()
        {
            var currentUser = GetUserDetails();
            return Ok(currentUser.Data);
        }

        //GET: api/users/settings
        [HttpGet("settings", Name = "GetUserSettings")]
        public IActionResult GetSettings()
        {
            var user = authService.GetCurrentUserData();
            var settings = queryBus.InvokeQuery<Settings>(new GetUserSettings(user.UserId));

            return Ok(settings.Data);
        }
    }
}
