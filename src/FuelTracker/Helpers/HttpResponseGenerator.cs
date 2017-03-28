using Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FuelTracker.Helpers
{
    public static class HttpResponseGenerator
    {
        public static IActionResult GetCommandRequestResponse(HttpRequest request, ICommandResult commandResult, bool redirectToObject = false)
        {
            if (commandResult.CommandException != null)
            {
                //exception mapper
                var response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                return null;
            }

            return null;
        }
    }
}
