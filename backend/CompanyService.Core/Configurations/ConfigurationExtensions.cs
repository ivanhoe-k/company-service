using System;
using CompanyService.Core.Common;
using Microsoft.Extensions.Configuration;

namespace CompanyService.Core.Configurations
{
    public static class ConfigurationExtensions
    {
        public static T GetConfiguration<T>(this IConfiguration config, string? sectionName = default)
            where T : class, new()
        {
            config.ThrowIfNull();

            var section = config.GetSection(sectionName ?? typeof(T).Name);

            if (!section.Exists())
            {
                throw new NullReferenceException($"{typeof(T).Name} is not configured");
            }

            var targetConfiguration = new T();
            section.Bind(targetConfiguration);

            return targetConfiguration;
        }
    }
}
