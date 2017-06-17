using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuelTracker.Interfaces
{
    public interface IHttpResponseGenerator
    {
        IActionResult GenerateCommandResponse(ICommandResult commandResult, string identifier, string createdAtRouteName, object data);
        IActionResult GenerateQueryResponse<T>(IQueryResult<T> queryResult);
    }
}
