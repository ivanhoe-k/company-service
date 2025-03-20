using CompanyService.Core.Common;
using CompanyService.Domain.Models;
using CompanyService.Persistence.Models;

namespace CompanyService.Persistence.Mappers
{
    public static class DomainToPersistenceMapper
    {
        public static CompanyEntity Map(Company company)
        {
            company.ThrowIfNull();

            return new CompanyEntity
            {
                Id = company.Id,
                Name = company.Name,
                ExchangeMicCode = company.Exchange.Value.MicCode,
                Ticker = company.Ticker,
                Isin = company.Isin.Value,
                Website = company.Website
            };
        }
    }
}
