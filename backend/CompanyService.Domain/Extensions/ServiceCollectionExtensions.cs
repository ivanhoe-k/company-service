using CompanyService.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyService.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddScoped<ICompanyManagerService, CompanyManagerService>();

            return services;
        }
    }
}
