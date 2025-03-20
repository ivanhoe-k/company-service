using CompanyService.Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyService.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IStockExchangeProvider, DummyStockExchangeProvider>();

            return services;
        }
    }
}
