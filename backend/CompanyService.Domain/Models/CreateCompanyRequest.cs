namespace CompanyService.Domain.Models
{
    public sealed record CreateCompanyRequest(string Name, string ExchangeName, string Ticker, string Isin, string? Website);
}
