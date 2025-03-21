using CompanyService.Api.Models;
using CompanyService.Core.Common;
using CompanyService.Domain.Models;

namespace CompanyService.Api.Mappers
{
    public static class ApiToDomainMapper
    {
        public static CreateCompanyRequest Map(CreateCompanyApiRequest apiRequest)
        {
            apiRequest.ThrowIfNull();

            return new CreateCompanyRequest(
                Name: apiRequest.Name,
                ExchangeName: apiRequest.ExchangeName,
                Ticker: apiRequest.Ticker,
                Isin: apiRequest.Isin.Value,
                Website: apiRequest.Website);
        }

        public static UpdateCompanyRequest Map(UpdateCompanyApiRequest apiRequest)
        {
            apiRequest.ThrowIfNull();

            return new UpdateCompanyRequest(
                Name: apiRequest.Name,
                ExchangeName: apiRequest.ExchangeName,
                Ticker: apiRequest.Ticker,
                Website: apiRequest.Website);
        }
    }
}
