using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyService.Core.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace CompanyService.Core.Web.StartupJobs
{
    public class StartupJobStartupFilter : IStartupFilter
    {
        private readonly ILogger<StartupJobStartupFilter> _logger;
        private readonly IEnumerable<IStartupJob> _startupJobs;

        public StartupJobStartupFilter(
            ILogger<StartupJobStartupFilter> logger,
            IEnumerable<IStartupJob> startupJobs)
        {
            logger.ThrowIfNull();
            startupJobs.ThrowIfNull();

            _logger = logger;
            _startupJobs = startupJobs;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            next.ThrowIfNull();

            _logger.LogDebug("Starting to execute startup jobs");

            var task = Task.WhenAll(_startupJobs.Select(job => ExecuteStartupJobAsync(job)));
            task.Wait();

            _logger.LogDebug("Finished startup jobs execution");

            return next;
        }

        private async Task ExecuteStartupJobAsync(IStartupJob job)
        {
            job.ThrowIfNull();

            _logger.LogDebug("Executing startup job: {JobName}", job.GetType().Name);
            await job.ExecuteAsync();
        }
    }
}
