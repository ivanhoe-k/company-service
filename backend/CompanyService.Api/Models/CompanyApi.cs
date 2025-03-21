using System;

namespace CompanyService.Api.Models
{
    public sealed record CompanyApi(
         Guid Id, string Name, string Exchange, string Ticker, string Isin, string? Website);
}
