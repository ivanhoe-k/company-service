using System.Collections.Generic;

namespace CompanyService.Core.Configurations.Validation
{
    public interface IConfigurationsValidator
    {
        void Validate(IEnumerable<IValidatableConfiguration> validatableConfigurations);
    }
}
