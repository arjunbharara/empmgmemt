using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using EmplyeMgm.Models;
namespace EmplyeMgm.Middlewares
{
    public class CustsomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustsomExceptionMiddleware> _logger;

        public CustsomExceptionMiddleware(RequestDelegate next, ILogger<CustsomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
               /* if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    await HandleStatusCodeAsync(httpContext, HttpStatusCode.NotFound);
                }*/
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Log the exception or handle it according to your needs.
            var result = new ErrorViewModel
            {
                RequestId = context.TraceIdentifier,
                StatusCode = context.Response.StatusCode,
                ErrorMessage = exception.Message
            };
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                return context.Response.WriteAsJsonAsync(result);
            }
            else
            {
                context.Response.Redirect("/Home/Error");
                return Task.CompletedTask;
            }
           // return context.Response.WriteAsJsonAsync(result);
        }

       /* private Task HandleStatusCodeAsync(HttpContext context, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorViewModel = new ErrorViewModel
            {
                RequestId = context.TraceIdentifier,
                ErrorMessage = statusCode == HttpStatusCode.NotFound ? "Page not found." : "An error occurred.",
                StatusCode = (int)statusCode
            };
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                return context.Response.WriteAsJsonAsync(errorViewModel);
            }
            else
            {
                context.Response.Redirect("/Home/Error");
                return Task.CompletedTask;
            }
           // return context.Response.WriteAsJsonAsync(errorViewModel);
        }*/
    }
}
