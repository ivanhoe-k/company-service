using System;
using System.ComponentModel.DataAnnotations;
using CompanyService.Infrastructure.Configurations.Validation;
using CompanyService.Infrastructure.Validation.Attributes;

namespace CompanyService.Infrastructure.Configurations
{
    public class ServiceConfiguration : IValidatableConfiguration
    {
        private static readonly Guid _instanceId = Guid.NewGuid();

        [NotEmptyGuid]
        public Guid InstanceId { get; } = _instanceId;

        [Required]
        public string Name { get; set; }
    }
}
