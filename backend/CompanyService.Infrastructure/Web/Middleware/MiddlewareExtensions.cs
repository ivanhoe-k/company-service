using CompanyService.Infrastructure.Common;
using CompanyService.Infrastructure.Web.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace CompanyService.Infrastructure.Web.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelationIdLogging(this IApplicationBuilder builder)
            => builder.UseMiddleware<CorrelationIdMiddleware>();

        public static IApplicationBuilder UseServiceContextLogging(this IApplicationBuilder builder)
            => builder.UseMiddleware<ServiceContextMiddleware>();

        public static IApplicationBuilder UseDefaultExceptionLogger(this IApplicationBuilder builder)
            => builder.UseMiddleware<DefaultExceptionLoggingMiddleware>();

        public static WebApplication UseSwaggerUI(this WebApplication app, WebAppConfiguration appConfiguration)
        {
            appConfiguration.ThrowIfNull();

            if (app.Environment.IsProduction() || app.Environment.IsStaging())
            {
                return app;
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint(
                    url: $"{appConfiguration.ApiConfiguration.GetUriFriendlyName()}/swagger.json",
                    name: appConfiguration.ApiConfiguration.Name));

            return app;
        }
    }
}
