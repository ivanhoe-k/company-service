using System.ComponentModel.DataAnnotations;
using CompanyService.Core.Configurations.Validation;

namespace CompanyService.Persistence.Configurations
{
    public sealed class SqlServerConfiguration : IValidatableConfiguration
    {
        [Required]
        public string ConnectionString { get; set; }
    }
}
