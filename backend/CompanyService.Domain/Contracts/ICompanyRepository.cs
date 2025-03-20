using System;
using System.Threading;
using System.Threading.Tasks;
using CompanyService.Core.Models;
using CompanyService.Domain.Errors;
using CompanyService.Domain.Models;

namespace CompanyService.Domain.Contracts
{
    /// <summary>
    /// Port interface for persistence operations related to Company entities.
    /// Defines a contract for persistence adapters to implement.
    /// </summary>
    public interface ICompanyRepository
    {
        Task<Result<CompanyError>> SaveAsync(Company company, CancellationToken cancellationToken);

        Task<Result<CompanyError>> UpdateAsync(Company company, CancellationToken cancellationToken);

        Task<Result<CompanyError>> DeleteAsync(Guid id, CancellationToken cancellationToken);

        Task<Result<CompanyError, Company>> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<Result<CompanyError, Company>> GetByIsinAsync(Isin isin, CancellationToken cancellationToken);

        Task<Result<CompanyError, Page<Company>>> GetCompaniesAsync(GetCompaniesRequest request, CancellationToken cancellationToken);

        Task<Result<CompanyError, bool>> ExistsByIsinAsync(string isin, CancellationToken cancellationToken);
    }
}
