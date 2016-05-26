using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ASPNETCoreRC2Demo
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next; 
        private readonly ILogger _logger;
        private readonly IDemoInterface _demo;
        public CustomMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IDemoInterface demo)
        {
            _next = next;
            _demo = demo;
            _logger = loggerFactory.CreateLogger<CustomMiddleware>();
        }
        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation($"{_demo.GetText} (Begin Invoke): Handling request: " + context.Request.Path);
            await _next.Invoke(context);
            _logger.LogInformation($"{_demo.GetText} (End Invoke): Finished handling request.");
        }
    }
	    public static class CustomMiddlewareExtensions
	    {
	        public static IApplicationBuilder UseDemoCustomMiddleware(this IApplicationBuilder builder)
	        {
	            return builder.UseMiddleware<CustomMiddleware>();
	        }
	    }
}