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

        public UserController(ICommandSender commandBus, IQuerySender queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;

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

        private IQueryResult<Settings> GetUserSettings()
        {
            var query = new GetUserSettings(HttpContext.GetCurrentUserId());
            var result = queryBus.InvokeQuery<Settings>(query);

            return result;
        }

        private IActionResult HandleQueryStatus<T>(IQueryResult<T> userDetailsQueryResult)
        {
            if (userDetailsQueryResult.QueryStatus == ActionStatus.BadRequest)
                return BadRequest(userDetailsQueryResult.ExceptionMessage);
            else if (userDetailsQueryResult.QueryStatus == ActionStatus.NotFound)
                return NotFound(userDetailsQueryResult.ExceptionMessage);
            else if (userDetailsQueryResult.QueryStatus == ActionStatus.Failure)
                return StatusCode(500, userDetailsQueryResult.ExceptionMessage);
            else return null;
        }

        [AllowAnonymous]
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
                        result.Data
                        );
                }
                else
                {
                    return StatusCode(500, commandResult.ExceptionMessage);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody]PutUser model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = GetUserDetails();

                var state = HandleQueryStatus(currentUser);
                if (state != null)
                    return state;

                var command = new UpdateUser(new Guid(currentUser.Data.Id), model.FirstName, model.LastName);
                commandBus.AddCommand(command);

                var commandResult = commandBus.InvokeCommandsQueue();

                if (commandResult.Status == ActionStatus.Success)
                {
                    var result = GetUserDetails();

                    return CreatedAtRoute(
                        "GetUser",
                        new { firstName = command.FirstName, lastName = command.LastName },
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
        [HttpGet(Name = "GetUser")]
        public IActionResult GetUser()
        {
            try
            {
                var currentUser = GetUserDetails();

                var state = HandleQueryStatus(currentUser);
                if (state != null)
                    return state;
                else
                    return Ok(currentUser.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //GET: api/users/settings
        [HttpGet("settings", Name = "GetUserSettings")]
        public IActionResult GetSettings()
        {
            try
            {

                var settings = GetUserSettings();
                var state = HandleQueryStatus(settings);
                if (state != null)
                    return state;

                return Ok(settings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
