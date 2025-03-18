using System.Threading.Tasks;
using CompanyService.Infrastructure.Common;
using CompanyService.Infrastructure.Configurations;
using CompanyService.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace CompanyService.Infrastructure.Web
{
    public sealed class ServiceContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ServiceConfiguration _serviceConfiguration;

        public ServiceContextMiddleware(RequestDelegate next, ServiceConfiguration serviceConfiguration)
        {
            next.ThrowIfNull();
            serviceConfiguration.ThrowIfNull();

            _next = next;
            _serviceConfiguration = serviceConfiguration;
        }

        public async Task Invoke(HttpContext context)
        {
            context.ThrowIfNull();

            using (LogContext.PushProperty(LoggingConstants.ServiceContext, _serviceConfiguration, true))
            {
                await _next.Invoke(context);
            }
        }
    }
}
