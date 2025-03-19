using System;
using System.Collections.Generic;
using CompanyService.Core.Common;
using CompanyService.Core.Configurations.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace CompanyService.Core.Web.Validation
{
    public class ConfigurationsValidationStartupFilter : IStartupFilter
    {
        private readonly ILogger<ConfigurationsValidationStartupFilter> _logger;
        private readonly IConfigurationsValidator _configurationsValidator;
        private readonly IEnumerable<IValidatableConfiguration> _validatableConfigurations;

        public ConfigurationsValidationStartupFilter(
                ILogger<ConfigurationsValidationStartupFilter> logger,
                IConfigurationsValidator configurationsValidator,
                IEnumerable<IValidatableConfiguration> validatableConfigurations)
        {
            logger.ThrowIfNull();
            configurationsValidator.ThrowIfNull();
            validatableConfigurations.ThrowIfNull();

            _logger = logger;
            _configurationsValidator = configurationsValidator;
            _validatableConfigurations = validatableConfigurations;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            next.ThrowIfNull();

            _logger.LogInformation("Start configuration validation");

            _configurationsValidator.Validate(_validatableConfigurations);

            // don't alter the configuration
            return next;
        }
    }
}
