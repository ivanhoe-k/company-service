using System;
using System.Threading;
using System.Threading.Tasks;
using CompanyService.Core.Models;
using CompanyService.Domain.Errors;
using CompanyService.Domain.Models;

namespace CompanyService.Domain.Services
{
    /// <summary>
    /// Core domain service for managing companies.
    /// 
    /// - This is not a port in Hexagonal Architecture terms, as it does not define an integration point.
    /// - Instead, it is a self-sufficient domain service, encapsulating pure business logic.
    /// - Placed inside `Services/` instead of `Contracts/` because it is internal domain logic, 
    ///   not an external-facing contract.
    /// - If multiple services were introduced, we could create `Services/Abstractions/`
    ///   to separate interfaces for better organization.
    /// </summary>
    public interface ICompanyManagerService
    {
        Task<Result<CompanyError, Company>> CreateCompanyAsync(CreateCompanyRequest createRequest, CancellationToken cancellationToken);

        Task<Result<CompanyError, Company>> GetCompanyByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<Result<CompanyError, Company>> GetCompanyByIsinAsync(string isin, CancellationToken cancellationToken);

        Task<Result<CompanyError, Page<Company>>> GetCompaniesAsync(GetCompaniesRequest request, CancellationToken cancellationToken);

        Task<Result<CompanyError, Company>> UpdateCompanyAsync(Guid id, UpdateCompanyRequest updateRequest, CancellationToken cancellationToken);
    }
}
