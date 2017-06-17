using Common.Enums;
using Common.Interfaces;
using FuelTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure
{
    public class HttpResponseGenerator : ControllerBase, IHttpResponseGenerator
    {
        public IActionResult GenerateCommandResponse(ICommandResult commandResult, string identifier, string createdAtRouteName, object data)
        {
            switch (commandResult.Status)
            {
                case ActionStatus.Success:
                    return CreatedAtRoute(
                        createdAtRouteName,
                        new { engineId = identifier },
                        data);
                case ActionStatus.NotFound:
                    return NotFound(commandResult.ExceptionMessage);
                case ActionStatus.BadRequest:
                    return BadRequest(commandResult.ExceptionMessage);
                case ActionStatus.Failure:
                default:
                    return StatusCode(500, commandResult.ExceptionMessage);
            }
        }

        public IActionResult GenerateQueryResponse<T>(IQueryResult<T> queryResult)
        {
            switch (queryResult.QueryStatus)
            {
                case ActionStatus.Success:
                    return Ok(queryResult);
                case ActionStatus.NotFound:
                    return NotFound(queryResult.ExceptionMessage);
                case ActionStatus.BadRequest:
                    return BadRequest(queryResult.ExceptionMessage);
                case ActionStatus.Failure:
                default:
                    return StatusCode(500, queryResult.ExceptionMessage);
            }
        }
    }
}
