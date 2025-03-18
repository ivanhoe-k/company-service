using System;
using System.Threading.Tasks;
using CompanyService.Infrastructure.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CompanyService.Infrastructure.Web.Middleware
{
    public class DefaultExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<DefaultExceptionLoggingMiddleware> _logger;

        public DefaultExceptionLoggingMiddleware(RequestDelegate next, ILogger<DefaultExceptionLoggingMiddleware> logger)
        {
            next.ThrowIfNull();
            logger.ThrowIfNull();

            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                throw;
            }
        }
    }
}
