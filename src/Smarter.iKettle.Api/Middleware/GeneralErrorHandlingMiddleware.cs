using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Smarter.iKettle.Api.Middleware
{
    public class GeneralErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostingEnvironment env;

        public GeneralErrorHandlingMiddleware(RequestDelegate next, IHostingEnvironment env)
        {
            this.next = next;
            this.env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(Exception ex)
            {
                await HandleGeneralException(context, ex);
            }
        }

        private Task HandleGeneralException(HttpContext context, Exception exception) =>
             HandleExceptionResponse(HttpStatusCode.InternalServerError, context, exception);

        private Task HandleExceptionResponse(HttpStatusCode code, HttpContext context, Exception exception)
        {
            string result = env.IsDevelopment()
                ? JsonConvert.SerializeObject(new { error = exception.Message, stacktrace = exception.StackTrace })
                : JsonConvert.SerializeObject(new { error = exception.Message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}