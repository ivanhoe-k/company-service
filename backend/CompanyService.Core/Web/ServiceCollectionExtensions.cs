using System.Linq;
using System.Text;
using CompanyService.Core.Common;
using CompanyService.Core.Configurations;
using CompanyService.Core.Web.Configurations;
using CompanyService.Core.Web.StartupJobs;
using CompanyService.Core.Web.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace CompanyService.Core.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStartupJobs(this IServiceCollection services)
            => services.AddTransient<IStartupFilter, StartupJobStartupFilter>();

        public static IServiceCollection AddStartupJob<TStartupJob>(this IServiceCollection services)
              where TStartupJob : class, IStartupJob
        {
            services.AddTransient<TStartupJob>();
            services.AddTransient<IStartupJob>(_ => _.GetRequiredService<TStartupJob>());

            return services;
        }

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

                if (appConfiguration.AuthConfiguration.Enabled)
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

        public static IServiceCollection AddAuthentication(this IServiceCollection services, WebAppConfiguration appConfiguration)
        {
            appConfiguration.ThrowIfNull();
            if (!appConfiguration.AuthConfiguration.Enabled)
            {
                return services;
            }

            var key = Encoding.UTF8.GetBytes(appConfiguration.AuthConfiguration.JwtSecret!);
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false; // Disabled for local testing
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false, // Would be validated in a real app
                        ValidateAudience = false, // Would be validated in a real app
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            return services;
        }

        public static IServiceCollection AddCorsPolicyConfiguration(
            this IServiceCollection services, IConfiguration configuration, string policyName)
        {
            configuration.ThrowIfNull();
            policyName.ThrowIfNullOrWhiteSpace();

            var corsConfiguration = configuration.GetConfiguration<CorsPolicyConfiguration>();

            Log.Information("Allowed origins {AllowedOrigins}", corsConfiguration.AllowedOrigins);

            services.AddCors(options =>
                options.AddPolicy(
                    policyName,
                    policyBuilder => CondigureCorsPolicies(policyBuilder, corsConfiguration)));

            return services;

            void CondigureCorsPolicies(CorsPolicyBuilder policyBuilder, CorsPolicyConfiguration corsPolicyConfiguration)
            {
                policyBuilder.WithOrigins(corsPolicyConfiguration.AllowedOrigins.ToArray())
                             .AllowAnyMethod()
                             .AllowAnyHeader();

                if (corsPolicyConfiguration.AllowCredentials)
                {
                    policyBuilder.AllowCredentials();
                }
            }
        }
    }
}
