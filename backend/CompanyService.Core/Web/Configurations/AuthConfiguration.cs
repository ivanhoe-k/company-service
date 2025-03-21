using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CompanyService.Core.Configurations.Validation;

namespace CompanyService.Core.Web.Configurations
{
    public sealed class AuthConfiguration : IValidatableConfiguration, IValidatableObject
    {
        public bool Enabled { get; init; }

        public string? JwtSecret { get; init; } = default;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Enabled && string.IsNullOrWhiteSpace(JwtSecret))
            {
                yield return new ValidationResult("JwtSecret is required when AuthenticationEnabled is true", new[] { nameof(JwtSecret) });
            }
        }
    }
}
