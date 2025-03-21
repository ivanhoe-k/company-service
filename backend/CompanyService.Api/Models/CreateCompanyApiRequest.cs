using System.ComponentModel.DataAnnotations;

namespace CompanyService.Api.Models
{
    public sealed record CreateCompanyApiRequest(
        [Required, StringLength(30)]
        string Name,

        [Required, StringLength(30)]
        string ExchangeName,

        [Required, StringLength(10)]
        string Ticker,

        [Required]
        IsinApi Isin,

        [Url]
        string? Website);
}
