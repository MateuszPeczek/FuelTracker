using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FuelTracker.Interfaces
{
    public interface IHttpResponseGenerator
    {
        IActionResult GenerateCommandResponse(ICommandResult commandResult, string identifier, string createdAtRouteName, object data);
        IActionResult GenerateQueryResponse<T>(IQueryResult<T> queryResult);
    }
}
