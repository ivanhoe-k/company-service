namespace CompanyService.Api.Models
{
    public sealed record CompanyApiFilter(
        string? Name, string? Exchange, string? Ticker, IsinApi? Isin);
}
