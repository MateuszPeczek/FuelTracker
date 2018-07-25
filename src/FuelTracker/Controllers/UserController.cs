using Commands.UserCommands;
using Common.Enums;
using Common.Interfaces;
using FuelTracker.ApiModels.AuthorisationApiModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Queries.UserQueries;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelTracker.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/users")]
    
    public class UserController : Controller
    {
        private readonly ICommandSender commandBus;
        private readonly IQuerySender queryBus;

        public UserController(ICommandSender commandBus, IQuerySender queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;

        }

        private UserDetails GetUserDetails(Guid userId)
        {
            var query = new GetSingleUser(userId);
            var result = queryBus.InvokeQuery<UserDetails>(query);

            return result.Data;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody]PostUser model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddUser(model.Email, model.Password);
                commandBus.AddCommand(command);

                var commandResult = commandBus.InvokeCommandsQueue();

                if (commandResult.Status == ActionStatus.Success)
                {
                    var result = GetUserDetails(command.Id);

                    return CreatedAtRoute(
                        "GetUser",
                        new { userEmail = command.Email },
                        result
                        );
                }
                else
                {
                    return StatusCode(500, commandResult.ExceptionMessage);
                }
            }

            return BadRequest(ModelState);
        }

        //GET: api/users
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(Name = "GetUser")]
        public IActionResult GetUser()
        {
            var currentUser = HttpContext.User;

            if (currentUser == null || !currentUser.Claims.Any())
                return NotFound();

            var id = currentUser.Claims.First(c => c.Type == "UserId").Value;

            var user = GetUserDetails(new Guid(id));

            return Ok(user);
        }
    }
}
