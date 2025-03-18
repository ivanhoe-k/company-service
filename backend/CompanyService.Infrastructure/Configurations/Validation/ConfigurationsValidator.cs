using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CompanyService.Infrastructure.Common;
using Microsoft.Extensions.Logging;

namespace CompanyService.Infrastructure.Configurations.Validation
{
    public class ConfigurationsValidator : IConfigurationsValidator
    {
        private readonly ILogger<ConfigurationsValidator> _logger;

        public ConfigurationsValidator(ILogger<ConfigurationsValidator> logger)
        {
            logger.ThrowIfNull();

            _logger = logger;
        }

        public void Validate(IEnumerable<IValidatableConfiguration> validatableConfigurations)
        {
            validatableConfigurations.ThrowIfNull();

            var currentConfigurationName = string.Empty;

            try
            {
                foreach (var validatableConfiguration in validatableConfigurations)
                {
                    currentConfigurationName = validatableConfiguration.GetType().Name;
                    _logger.LogInformation("{ConfigurationName} validation", currentConfigurationName);

                    Validator.ValidateObject(
                        validatableConfiguration, new ValidationContext(validatableConfiguration), validateAllProperties: true);
                }
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "{ConfigurationName} is invalid: {ErrorDetails}", currentConfigurationName, ex.Message);

                throw;
            }
        }
    }
}
