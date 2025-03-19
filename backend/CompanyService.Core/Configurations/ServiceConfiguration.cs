using System;
using System.ComponentModel.DataAnnotations;
using CompanyService.Core.Configurations.Validation;
using CompanyService.Core.Validation.Attributes;

namespace CompanyService.Core.Configurations
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
