using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Serilog;

namespace Project.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RequestLogger
    {
        private readonly RequestDelegate _next;
       
        public RequestLogger(RequestDelegate next)
        {
            _next = next;
           
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                Log.Information($"Request: {context.Request.Method} {context.Request.Path} {context.Request.Body} {context.Request.Query}");
                await _next(context);
                Log.Information($"Response: {context.Response.StatusCode} {context.Response.Body}");
            }
            catch (Exception ex) {
                Log.Error($"Execprion:  {ex.Message} {context.Request.Path} {context.Request.Method} {context.Response.StatusCode}");
                throw ex;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RequestLoggerExtensions
    {
        public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLogger>();
        }
    }
}
