using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CompanyService.Infrastructure.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredWithNestedAttribute : ValidationAttribute
    {
        public bool AllowNullValue { get; set; }

        public bool AllowEmptyStrings { get; set; }

        public string PropertyName { get; }

        public RequiredWithNestedAttribute([CallerMemberName] string? propertyName = default)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        }

        public override bool IsValid(object? value)
        {
            if (value == null && AllowNullValue)
            {
                return true;
            }

            if (value is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    if (!ValidateNestedObject(item))
                    {
                        return false;
                    }
                }
            }

            return ValidateNestedObject(value);
        }

        private bool ValidateNestedObject(object? value)
        {
            if (value is null)
            {
                ErrorMessage = $"{PropertyName} cannot be null";
                return false;
            }

            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(value!, new ValidationContext(value), validationResults, true))
            {
                if (validationResults is null || validationResults.Count == 0)
                {
                    throw new Exception("Something went wrong during validation");
                }

                ErrorMessage = string.Join(",", validationResults.Select(_ => _.ErrorMessage));

                return false;
            }

            return true;
        }
    }
}
