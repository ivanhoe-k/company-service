using System.ComponentModel.DataAnnotations;

namespace CompanyService.Api.Models
{
    public record TokenRequest(
        [Required]
        string ClientId,

        [Required]
        string ClientSecret);
}
