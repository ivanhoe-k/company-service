using System.ComponentModel.DataAnnotations;
using CompanyService.Core.Configurations.Validation;

namespace CompanyService.Core.Web.Configurations
{
    public sealed class ApiConfiguration : IValidatableConfiguration
    {
        [Required]
        public string Name { get; init; }

        [Required]
        public string Version { get; init; }

        public string GetUriFriendlyName() => Name.ToLower().Trim().Replace(" ", "-");
    }
}
