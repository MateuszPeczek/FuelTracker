using CustomExceptions.GroupingIntefaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Infrastructure.ExceptionHandling
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            if (exception is IBadRequestException)
                response.StatusCode = (int)HttpStatusCode.BadRequest;
            else if (exception is INotFoundException)
                response.StatusCode = (int)HttpStatusCode.NotFound;
            else if (exception is IUnauthorizedException)
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            else
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await response.WriteAsync(JsonConvert.SerializeObject(new
            {           
                error = new
                {
                    message = exception.Message,
                    exception = exception.GetType().Name
                }
            }));
        }
    }
}
