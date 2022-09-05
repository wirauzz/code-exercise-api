using Logic.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Presentation.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private const string _jsonContentType = "application/json";

        private readonly RequestDelegate _next;
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var ErrorResponse = new MiddlewareResponse<string>(null);

            if (ex is LogicException)
            {
                ErrorResponse.status = (int)HttpStatusCode.OK;
                ErrorResponse.error.message = "Logic Exception" + Environment.NewLine + "Message: " + ex.Message + Environment.NewLine;
            }
            else
            {
                ErrorResponse.status = (int)HttpStatusCode.InternalServerError;
                ErrorResponse.error.message = "Internal Server Error";
            }

            var repositoryResult = new
            {
                context.Response.StatusCode,
                message = ex.Message,
            };

            context.Response.ContentType = _jsonContentType;
            context.Response.StatusCode = ErrorResponse.status;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(ErrorResponse));
        }
    }
}
