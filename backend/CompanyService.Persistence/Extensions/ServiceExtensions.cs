using CompanyService.Core.Configurations;
using CompanyService.Core.Web;
using CompanyService.Domain.Contracts;
using CompanyService.Infrastructure.Persistence.Repositories;
using CompanyService.Persistence.Configurations;
using CompanyService.Persistence.StartupJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyService.Persistence.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // SqlServer & EF configuration
            services.AddValidatableConfiguration<SqlServerConfiguration>(configuration);
            var sqlServerConfiguration = configuration.GetConfiguration<SqlServerConfiguration>();

            services.AddDbContext<CompanyDbContext>(options =>
            options.UseSqlServer(sqlServerConfiguration.ConnectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(CompanyDbContext).Assembly.FullName);
            }));

            // Provide adapter implementations for the domain repositories
            services.AddScoped<ICompanyRepository, CompanyRepository>();

            // Add startup jobs
            services.AddStartupJob<RunMigrationsStartupJob>();

            return services;
        }
    }
}
