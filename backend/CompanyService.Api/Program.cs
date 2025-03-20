using CompanyService.Application;
using CompanyService.Core.Web;
using CompanyService.Domain.Extensions;
using CompanyService.Persistence.Extensions;

await CompanyCoreApiService
    .Create()
    .ConfigureServices((services, configuration) =>
    {
        services.AddDomain();
        services.AddPersistence(configuration);
        services.AddApplication();
    })
    .RunAsync();