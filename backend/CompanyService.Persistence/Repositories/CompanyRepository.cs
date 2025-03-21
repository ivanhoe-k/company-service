using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CompanyService.Core.Common;
using CompanyService.Core.Models;
using CompanyService.Domain.Contracts;
using CompanyService.Domain.Errors;
using CompanyService.Domain.Models;
using CompanyService.Persistence;
using CompanyService.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CompanyService.Infrastructure.Persistence.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly CompanyDbContext _dbContext;
        private readonly ILogger<CompanyRepository> _logger;

        public CompanyRepository(CompanyDbContext dbContext, ILogger<CompanyRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result<CompanyError>> SaveAsync(Company company, CancellationToken cancellationToken)
        {
            company.ThrowIfNull();

            try
            {
                var companyEntity = DomainToPersistenceMapper.Map(company);
                await _dbContext.Companies.AddAsync(companyEntity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Result<CompanyError>.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving company {CompanyId}", company.Id);
                return Result<CompanyError>.Fail(CompanyError.SomethingWentWrong);
            }
        }

        public async Task<Result<CompanyError>> UpdateAsync(Company company, CancellationToken cancellationToken)
        {
            company.ThrowIfNull();

            var entity = await _dbContext.Companies.FindAsync([company.Id], cancellationToken);
            
            if (entity == null)
            {
                return Result<CompanyError>.Fail(CompanyError.NotFound);
            }

            var updatedEntity = DomainToPersistenceMapper.Map(company);
            _dbContext.Entry(entity).CurrentValues.SetValues(updatedEntity);

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Result<CompanyError>.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating company {CompanyId}", company.Id);
                return Result<CompanyError>.Fail(CompanyError.SomethingWentWrong);
            }
        }

        public async Task<Result<CompanyError, CompanyDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            id.ThrowIfEmpty();

            var entity = await _dbContext.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            return entity is null
                ? Result<CompanyError>.Fail<CompanyDto>(CompanyError.NotFound)
                : Result<CompanyError>.Ok(PersistenceToDomainMapper.Map(entity));
        }

        public async Task<Result<CompanyError, CompanyDto>> GetByIsinAsync(Isin isin, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Isin == isin.Value, cancellationToken);

            return entity is null
                ? Result<CompanyError>.Fail<CompanyDto>(CompanyError.NotFound)
                : Result<CompanyError>.Ok(PersistenceToDomainMapper.Map(entity));
        }

        public async Task<Result<CompanyError, bool>> ExistsByIsinAsync(string isin, CancellationToken cancellationToken)
        {
            var exists = await _dbContext.Companies.AnyAsync(c => c.Isin == isin, cancellationToken);
            return Result<CompanyError>.Ok(exists);
        }

        public async Task<Result<CompanyError, Page<CompanyDto>>> GetCompaniesAsync(
            GetCompaniesRequest request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull();

            var query = _dbContext.Companies.AsQueryable();

            // Apply filtering
            if (!string.IsNullOrWhiteSpace(request.Filter?.Name))
            {
                query = query.Where(c => c.Name.Contains(request.Filter.Name));
            }

            if (!string.IsNullOrWhiteSpace(request.Filter?.Exchange))
            {
                query = query.Where(c => c.ExchangeMicCode == request.Filter.Exchange);
            }

            if (!string.IsNullOrWhiteSpace(request.Filter?.Ticker))
            {
                query = query.Where(c => c.Ticker == request.Filter.Ticker);
            }

            if (request.Filter?.Isin is not null)
            {
                query = query.Where(c => c.Isin == request.Filter.Isin.Value);
            }

            // Apply sorting
            query = request.SortOrder switch
            {
                SortOrder.Asc => query.OrderBy(c => c.Name),
                SortOrder.Desc => query.OrderByDescending(c => c.Name),
                _ => query
            };

            // Get total count first
            var totalCount = await query.CountAsync(cancellationToken);

            // Calculate pagination
            var pageSize = request.PageSize;
            var currentPage = request.PageNumber;
            var skip = (currentPage - 1) * pageSize;

            var items = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            // Map to DTOs
            var mappedItems = items.Select(PersistenceToDomainMapper.Map).ToList();

            // Build PageInfo
            var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);
            var pageInfo = new PageInfo(
                currentPage: currentPage,
                pageSize: pageSize,
                totalPages: totalPages,
                hasNextPage: currentPage < totalPages,
                hasPreviousPage: currentPage > 1);

            var page = new Page<CompanyDto>(
                items: mappedItems,
                totalCount: totalCount,
                pageInfo: pageInfo);

            return Result<CompanyError>.Ok(page);
        }
    }
}
