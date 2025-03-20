using CompanyService.Api.Models;
using CompanyService.Core.Common;
using CompanyService.Domain.Models;

namespace CompanyService.Api.Mappers
{
    public static class DomainToApiMapper
    {
        public static CompanyApi Map(Company company)
        {
            company.ThrowIfNull();

            return new CompanyApi(
                Id: company.Id,
                Name: company.Name,
                Exchange: company.Exchange.Value.ExchangeName,
                Ticker: company.Ticker,
                Isin: company.Isin.Value,
                Website: company.Website?.ToString());
        }
    }
}
