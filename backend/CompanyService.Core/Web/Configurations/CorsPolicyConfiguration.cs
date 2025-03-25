using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CompanyService.Core.Configurations.Validation;

namespace CompanyService.Core.Web.Configurations
{
    public class CorsPolicyConfiguration : IValidatableConfiguration
    {
        [Required]
        [MinLength(1)]
        public IReadOnlyCollection<string> AllowedOrigins { get; init; }

        public bool AllowCredentials { get; init; } = false;
    }
}
