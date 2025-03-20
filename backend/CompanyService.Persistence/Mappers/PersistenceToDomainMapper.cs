using System.Collections.Generic;
using System.Linq;
using CompanyService.Core.Common;
using CompanyService.Core.Models;
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

        public static IReadOnlyCollection<Edge<CompanyDto>> Map(IReadOnlyCollection<CompanyEntity> entities)
        {
            entities.ThrowIfNull();

            return entities
                .Select(entity => new Edge<CompanyDto>(Map(entity), entity.Id.ToString()))
                .ToList();
        }
    }
}
