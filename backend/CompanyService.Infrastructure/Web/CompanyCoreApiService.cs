using System;
using System.Net.Http;
using System.Threading.Tasks;
using CompanyService.Infrastructure.Common;
using CompanyService.Infrastructure.Configurations;
using CompanyService.Infrastructure.Configurations.Validation;
using CompanyService.Infrastructure.Logging;
using CompanyService.Infrastructure.Web.Configurations;
using CompanyService.Infrastructure.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;

namespace CompanyService.Infrastructure.Web
{
    public class CompanyCoreApiService
    {
        public static CompanyCoreApiService Create() => new CompanyCoreApiService();

        public async Task RunAsync()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var builder = WebApplication.CreateBuilder();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            
            var logger = GetLogger<CompanyCoreApiService>();

            try
            {
                logger.LogInformation("Environment: {Environment}", environment);

                var serviceConfiguration = builder.Configuration.GetConfiguration<ServiceConfiguration>();
                var appConfiguration = builder.Configuration.GetConfiguration<WebAppConfiguration>();

                ValidateConfigurations(serviceConfiguration, appConfiguration);

                using var loggingContext = LogContext.PushProperty(LoggingConstants.ServiceContext, serviceConfiguration, true);

                logger.LogInformation("The service {ServiceName} starting up",  serviceConfiguration.Name);

                ConfigureServices(
                    services: builder.Services,
                    configuration: builder.Configuration,
                    appConfiguration: appConfiguration);

                ConfigureHost(builder.Host);

                var app = builder.Build();

                ConfigureMiddleware(
                    app: app,
                    configuration: builder.Configuration,
                    appConfiguration: appConfiguration);

                await app.RunAsync();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "[Critical] The application failed to start");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        protected virtual void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration,
            WebAppConfiguration appConfiguration)
        {
            services.ThrowIfNull();
            configuration.ThrowIfNull();
            appConfiguration.ThrowIfNull();

            services.AddConfigurationsValidator();
            services.AddConfigurationValidationStartupFilter();
            services.AddValidatableConfiguration<ServiceConfiguration>(configuration);
            services.AddValidatableConfiguration<WebAppConfiguration>(configuration);

            services.AddHttpClient<HttpClient>(client =>
            {
                client.DefaultRequestHeaders.Add(
                    LoggingConstants.CorrelationIdHeaderName,
                    CorrelationIdContext.GetCorrelationId() ?? Guid.NewGuid().ToString());
            });

            services
                .AddControllers(options =>
                {
                    options.SuppressAsyncSuffixInActionNames = false;
                    options.AllowEmptyInputInBodyModelBinding = true;
                })
                .AddNewtonsoftJson();

            services.AddSwagger(appConfiguration);
            services.AddProblemDetails();
        }

        protected virtual void ConfigureHost(ConfigureHostBuilder builder)
        {
            builder.ThrowIfNull();

            builder.UseSerilog();
        }

        protected virtual void ConfigureMiddleware(
            WebApplication app,
            IConfiguration configuration,
            WebAppConfiguration appConfiguration)
        {
            app.ThrowIfNull();
            configuration.ThrowIfNull();
            appConfiguration.ThrowIfNull();

            app.UsePathBase(appConfiguration.BasePath);
            app.UseServiceContextLogging();
            app.UseCorrelationIdLogging();

            app.UseSwaggerUI(appConfiguration);
            app.UseDefaultExceptionLogger();

            app.MapControllers();
        }

        /// <summary>
        /// Temporary logger creation method for use outside DI.
        /// Should normally happen only during startup and NOT be used elsewhere.
        /// </summary>
        /// <typeparam name="T">The type for which the logger is created.</typeparam>
        /// <returns>An instance of ILogger<typeparamref name="T"/>.</returns>
        private ILogger<T> GetLogger<T>()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            return loggerFactory.CreateLogger<T>();
        }

        private void ValidateConfigurations(params IValidatableConfiguration[] validatableConfigurations)
        {
            var validator = new ConfigurationsValidator(GetLogger<ConfigurationsValidator>());
            validator.Validate(validatableConfigurations);
        }
    }
}
