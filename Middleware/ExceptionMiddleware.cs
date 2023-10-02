using jonas.Application.Services.Exceptions;
using jonas.Extensions;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace jonas.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly IHostEnvironment _hostEnvironment;

        public ExceptionMiddleware(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Logger.log(ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var responseMessage = ex.Message;
            switch (ex)
            {
                case InvalidDateTimeException:
                    responseMessage = ex.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity; 
                    break;
                case BadHttpRequestException:
                    responseMessage = "General BadHttpRequestException";
                    context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    break;
                case EntityNotFoundException:
                    responseMessage = "Did not find the entity you were looking for";
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case DBConcurrencyException:
                    responseMessage = "Database concurrency exception";
                    break;
                case DbException:
                    responseMessage = "Database not available";
                    break;
                default:
                    responseMessage = ex.Message;
                    break;
            }
            var response = _hostEnvironment.IsDevelopment()
                ? new CustomResponse(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new CustomResponse(context.Response.StatusCode, responseMessage);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}
