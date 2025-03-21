using System;
using System.Threading.Tasks;
using CompanyService.Core.Common;
using CompanyService.Core.Web.StartupJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CompanyService.Persistence.StartupJobs
{
    public sealed class RunMigrationsStartupJob : IStartupJob
    {
        private readonly ILogger<RunMigrationsStartupJob> _logger;
        private readonly IServiceProvider _serviceProvider;

        public RunMigrationsStartupJob(
            ILogger<RunMigrationsStartupJob> logger,
            IServiceProvider serviceProvider)
        {
            logger.ThrowIfNull();
            serviceProvider.ThrowIfNull();

            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Applying EF Core migrations...");

            // This startup job runs via IStartupFilter, which executes before the application is fully built.
            // At this stage, only transient services can be injected directly.
            // Since DbContext is scoped, we resolve it manually via IServiceProvider within a scope.
            // While this is generally considered an anti-pattern, it's a practical workaround in this context.
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CompanyDbContext>();
            await dbContext.Database.MigrateAsync();

            _logger.LogInformation("EF Core migrations applied.");
        }
    }
}
