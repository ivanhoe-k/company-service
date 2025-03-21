using System.Threading.Tasks;

namespace CompanyService.Core.Web.StartupJobs
{
    public interface IStartupJob
    {
        Task ExecuteAsync();
    }
}
