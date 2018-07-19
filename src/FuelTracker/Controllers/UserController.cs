using Commands.UserCommands;
using Common.Enums;
using Common.Interfaces;
using FuelTracker.ApiModels.AuthorisationApiModels;
using FuelTracker.Helpers;
using Microsoft.AspNetCore.Mvc;
using Queries.UserQueries;
using System;

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
                        new { userId = command.Id },
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

        //GET: api/users/{userId}
        [HttpGet("{userId}", Name = "GetSingleUser")]
        public IActionResult GetUser([ModelBinder(BinderType = typeof(CollectionModelBinder))]Guid userId)
        {
            var result = GetUserDetails(userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
