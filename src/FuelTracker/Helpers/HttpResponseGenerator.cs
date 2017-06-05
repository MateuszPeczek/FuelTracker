using Common.Enums;
using Common.Interfaces;
using FuelTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class HttpResponseGenerator : ControllerBase, IHttpResponseGenerator
    {
        public IActionResult GenerateCommandResponse(ICommandResult commandResult, )
        {
            switch (commandResult.Status)
            {
                case ActionStatus.Success:
                    return 
                case ActionStatus.NotFound:
                    break;
                case ActionStatus.BadRequest:
                    break;
                case ActionStatus.Failure:
                default:
                    break;
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
