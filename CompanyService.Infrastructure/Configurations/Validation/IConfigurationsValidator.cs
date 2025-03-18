using System.Collections.Generic;

namespace CompanyService.Infrastructure.Configurations.Validation
{
    public interface IConfigurationsValidator
    {
        void Validate(IEnumerable<IValidatableConfiguration> validatableConfigurations);
    }
}
