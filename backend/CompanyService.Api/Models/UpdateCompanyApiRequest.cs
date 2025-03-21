using System.ComponentModel.DataAnnotations;

namespace CompanyService.Api.Models
{
    public sealed record UpdateCompanyApiRequest(
        [StringLength(30)]
        string? Name,

        [StringLength(30)]
        string? ExchangeName,

        [StringLength(10)]
        string? Ticker,

        [Url]
        string? Website);
}
