using System;
using System.Linq;
using System.Threading.Tasks;
using CompanyService.Infrastructure.Common;
using CompanyService.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace CompanyService.Infrastructure.Web.Middleware
{
    public sealed class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            next.ThrowIfNull();

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.ThrowIfNull();

            context.Request.Headers.TryGetValue(LoggingConstants.CorrelationIdHeaderName, out var correlationIds);

            var correlationId = correlationIds.FirstOrDefault() ?? Guid.NewGuid().ToString();

            CorrelationIdContext.SetCorrelationId(correlationId);

            using (LogContext.PushProperty(LoggingConstants.CorrelationIdPropertyName, correlationId))
            {
                await _next.Invoke(context);
            }
        }
    }
}
