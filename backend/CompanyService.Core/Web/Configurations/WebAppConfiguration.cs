using CompanyService.Core.Configurations.Validation;
using CompanyService.Core.Validation.Attributes;

namespace CompanyService.Core.Web.Configurations
{
    public sealed class WebAppConfiguration : IValidatableConfiguration
    {
        public string BasePath { get; init; } = string.Empty;

        [RequiredWithNested]
        public AuthConfiguration AuthConfiguration { get; init; } 

        [RequiredWithNested]
        public ApiConfiguration ApiConfiguration { get; init; }
    }
}
