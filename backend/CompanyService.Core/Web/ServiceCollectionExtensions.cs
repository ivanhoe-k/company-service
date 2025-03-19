using System.Linq;
using CompanyService.Core.Common;
using CompanyService.Core.Web.Configurations;
using CompanyService.Core.Web.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CompanyService.Core.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigurationValidationStartupFilter(this IServiceCollection services)
            => services.AddTransient<IStartupFilter, ConfigurationsValidationStartupFilter>();

        public static IServiceCollection AddSwagger(this IServiceCollection services, WebAppConfiguration appConfiguration)
        {
            appConfiguration.ThrowIfNull();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    name: appConfiguration.ApiConfiguration.GetUriFriendlyName(),
                    info: new OpenApiInfo
                    {
                        Title = appConfiguration.ApiConfiguration.Name,
                        Version = appConfiguration.ApiConfiguration.Version
                    });

                if (appConfiguration.AuthenticationEnabled)
                {
                    options.AddSecurityDefinition(
                        name: JwtBearerDefaults.AuthenticationScheme,
                        securityScheme: new OpenApiSecurityScheme
                        {
                            In = ParameterLocation.Header,
                            Description = "Please enter a valid token",
                            Name = "Authorization",
                            Type = SecuritySchemeType.Http,
                            BearerFormat = "JWT",
                            Scheme = JwtBearerDefaults.AuthenticationScheme
                        });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            Enumerable.Empty<string>().ToList()
                        }
                    });
                }
            });

            return services;
        }
    }
}
