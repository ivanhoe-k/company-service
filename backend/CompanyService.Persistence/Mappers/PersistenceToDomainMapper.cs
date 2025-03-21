using CompanyService.Core.Common;
using CompanyService.Domain.Models;
using CompanyService.Persistence.Models;

namespace CompanyService.Persistence.Mappers
{
    public static class PersistenceToDomainMapper
    {
        public static CompanyDto Map(CompanyEntity entity)
        {
            entity.ThrowIfNull();

            return new CompanyDto(
                id: entity.Id,
                name: entity.Name,
                exchangeMicCode: entity.ExchangeMicCode,
                ticker: entity.Ticker,
                isin: entity.Isin,
                website: entity.Website);
        }
    }
}
