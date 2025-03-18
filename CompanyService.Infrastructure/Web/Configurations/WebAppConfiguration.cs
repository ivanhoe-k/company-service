﻿using CompanyService.Infrastructure.Configurations.Validation;
using CompanyService.Infrastructure.Validation.Attributes;

namespace CompanyService.Infrastructure.Web.Configurations
{
    public sealed class WebAppConfiguration : IValidatableConfiguration
    {
        public string BasePath { get; init; } = string.Empty;

        public bool AuthenticationEnabled { get; init; }

        [RequiredWithNested]
        public ApiConfiguration ApiConfiguration { get; init; }
    }
}
