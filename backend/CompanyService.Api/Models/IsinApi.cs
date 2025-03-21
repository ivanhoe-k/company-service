using System.ComponentModel.DataAnnotations;

namespace CompanyService.Api.Models
{
    public sealed record IsinApi
    {
        [RegularExpression(@"^[A-Z]{2}[A-Z0-9]{9}[0-9]$", ErrorMessage = "Invalid ISIN format.")]
        public string Value { get; init; }
    }
}
