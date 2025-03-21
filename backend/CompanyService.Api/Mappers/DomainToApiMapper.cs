using System.Collections.Generic;
using System.Linq;
using CompanyService.Api.Models;
using CompanyService.Core.Common;
using CompanyService.Core.Models;
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

        public static Page<CompanyApi> Map(Page<Company> page)
        {
            page.ThrowIfNull();

            return new Page<CompanyApi>(
                items: Map(page.Items),
                totalCount: page.TotalCount,
                pageInfo: page.PageInfo);
        }

        public static IReadOnlyCollection<CompanyApi> Map(IReadOnlyCollection<Company> companies)
        {
            companies.ThrowIfNull();

            return companies.Select(Map).ToList();
        }
    }
}
