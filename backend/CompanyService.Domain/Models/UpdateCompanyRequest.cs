namespace CompanyService.Domain.Models
{
    public sealed record UpdateCompanyRequest(
        string? Name, string? ExchangeName, string? Ticker, string? Website);
}
