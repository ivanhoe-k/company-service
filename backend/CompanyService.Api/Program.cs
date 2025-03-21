using CompanyService.Api.Auth;
using CompanyService.Application;
using CompanyService.Core.Web;
using CompanyService.Domain.Extensions;
using CompanyService.Persistence.Extensions;
using Microsoft.Extensions.DependencyInjection;

await CompanyCoreApiService
    .Create()
    .ConfigureServices((services, configuration) =>
    {
        services.AddDomain();
        services.AddPersistence(configuration);
        services.AddApplication();
        services.AddScoped<ITokenService, TokenService>();
    })
    .RunAsync();