using System;
using CompanyService.Core.Common;
using CompanyService.Core.Configurations.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CompanyService.Core.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddValidatableConfiguration<TValidatableConfiguration>(
            this IServiceCollection services, IConfiguration configuration)
        where TValidatableConfiguration : class, IValidatableConfiguration, new()
        {
            configuration.ThrowIfNull();

            var validatableConfiguration = configuration.GetConfiguration<TValidatableConfiguration>();

            return services.AddValidatableConfiguration(validatableConfiguration);
        }

        public static IServiceCollection AddValidatableConfiguration<TValidatableConfiguration>(
           this IServiceCollection services, IConfiguration configuration, out TValidatableConfiguration configurationInstance)
       where TValidatableConfiguration : class, IValidatableConfiguration, new()
        {
            configuration.ThrowIfNull();

            configurationInstance = configuration.GetConfiguration<TValidatableConfiguration>();

            return services.AddValidatableConfiguration(configurationInstance);
        }

        public static IServiceCollection AddValidatableConfiguration<TValidatableConfiguration>(
            this IServiceCollection services, TValidatableConfiguration configurationInstance)
        where TValidatableConfiguration : class, IValidatableConfiguration, new()
        {
            if (configurationInstance is null)
            {
                var configurationName = typeof(TValidatableConfiguration).FullName;
                throw new InvalidOperationException($"{configurationName} is not configured");
            }

            services.AddSingleton(configurationInstance);
            services.Add(
                new ServiceDescriptor(typeof(IValidatableConfiguration), configurationInstance));

            return services;
        }

        public static IServiceCollection AddConfigurationsValidator(this IServiceCollection services)
        {
            services.TryAddSingleton<IConfigurationsValidator, ConfigurationsValidator>();

            return services;
        }
    }
}
