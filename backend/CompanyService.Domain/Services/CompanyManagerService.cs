using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CompanyService.Core.Common;
using CompanyService.Core.Models;
using CompanyService.Domain.Contracts;
using CompanyService.Domain.Errors;
using CompanyService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace CompanyService.Domain.Services
{
    public class CompanyManagerService : ICompanyManagerService
    {
        private readonly ILogger<CompanyManagerService> _logger;
        private readonly ICompanyRepository _companyRepository;
        private readonly IStockExchangeProvider _stockExchangeProvider;

        public CompanyManagerService(
            ILogger<CompanyManagerService> logger,
            ICompanyRepository companyRepository,
            IStockExchangeProvider stockExchangeProvider)
        {
            logger.ThrowIfNull();
            companyRepository.ThrowIfNull();
            stockExchangeProvider.ThrowIfNull();

            _logger = logger;
            _companyRepository = companyRepository;
            _stockExchangeProvider = stockExchangeProvider;
        }

        public async Task<Result<CompanyError, Company>> CreateCompanyAsync(
            CreateCompanyRequest createRequest, CancellationToken cancellationToken)
        {
            createRequest.ThrowIfNull();

            _logger.LogInformation("Attempting to create a new company with ISIN {Isin}", createRequest.Isin);

            var existsByIsinResult = await ExistsByIsinAsync(createRequest.Isin, cancellationToken);
            if (existsByIsinResult.Failed)
            {
                return Result<CompanyError>.Fail<Company>(existsByIsinResult.Error!);
            }

            var exchangeLookupByNameResult = await GetStockExchangeLookupByNameAsync(cancellationToken);
            if (exchangeLookupByNameResult.Failed)
            {
                return Result<CompanyError>.Fail<Company>(exchangeLookupByNameResult.Error!);
            }

            var companyResult = Company.Create(createRequest, exchangeLookupByNameResult.Value);
            if (companyResult.Failed)
            {
                _logger.LogWarning("Failed to create company due to validation error: {Error}", companyResult.Error);
                return companyResult;
            }

            var saveResult = await _companyRepository.SaveAsync(companyResult.Value, cancellationToken);
            if (saveResult.Failed)
            {
                _logger.LogError("Failed to save company with ISIN {Isin}. Error: {Error}", createRequest.Isin, saveResult.Error);
                return Result<CompanyError>.Fail<Company>(saveResult.Error!);
            }

            _logger.LogInformation("Successfully created company {CompanyId} with ISIN {Isin}", companyResult.Value.Id, createRequest.Isin);
            
            return companyResult;
        }

        public async Task<Result<CompanyError, Company>> GetCompanyByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            id.ThrowIfEmpty();

            _logger.LogInformation("Fetching company by ID: {CompanyId}", id);

            var companyTask = _companyRepository.GetByIdAsync(id, cancellationToken);
            var exchangeLookupTask = GetStockExchangeLookupByMicCodeAsync(cancellationToken);

            await Task.WhenAll(companyTask, exchangeLookupTask);

            if (companyTask.Result.Failed)
            {
                _logger.LogWarning("Failed to find company with ID: {CompanyId}", id);
                return Result<CompanyError>.Fail<Company>(companyTask.Result.Error!);
            }

            if (exchangeLookupTask.Result.Failed)
            {
                return Result<CompanyError>.Fail<Company>(exchangeLookupTask.Result.Error!);
            }

            return Company.Create(companyTask.Result.Value, exchangeLookupTask.Result.Value);
        }

        public async Task<Result<CompanyError, Company>> GetCompanyByIsinAsync(string isin, CancellationToken cancellationToken)
        {
            isin.ThrowIfNullOrWhiteSpace();

            _logger.LogInformation("Fetching company by ISIN: {Isin}", isin);

            var isinValidationResult = Isin.Create(isin);
            if (isinValidationResult.Failed)
            {
                _logger.LogWarning("Invalid ISIN format: {Isin}", isin);
                return Result<CompanyError>.Fail<Company>(isinValidationResult.Error!);
            }

            var companyTask = _companyRepository.GetByIsinAsync(isinValidationResult.Value, cancellationToken);
            var exchangeLookupTask = GetStockExchangeLookupByMicCodeAsync(cancellationToken);

            await Task.WhenAll(companyTask, exchangeLookupTask);

            if (companyTask.Result.Failed)
            {
                _logger.LogWarning("Failed to find company with ISIN: {Isin}", isin);
                return Result<CompanyError>.Fail<Company>(companyTask.Result.Error!);
            }

            if (exchangeLookupTask.Result.Failed)
            {
                return Result<CompanyError>.Fail<Company>(exchangeLookupTask.Result.Error!);
            }

            return Company.Create(companyTask.Result.Value, exchangeLookupTask.Result.Value);
        }

        public async Task<Result<CompanyError, Page<Company>>> GetCompaniesAsync(
            GetCompaniesRequest request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull();

            _logger.LogInformation("Fetching companies for request: {Request}", request);

            var companiesDtoTask = _companyRepository.GetCompaniesAsync(request, cancellationToken);
            var exchangeLookupTask = GetStockExchangeLookupByMicCodeAsync(cancellationToken);

            await Task.WhenAll(companiesDtoTask, exchangeLookupTask);

            if (companiesDtoTask.Result.Failed)
            {
                _logger.LogError("Failed to retrieve companies. Error: {Error}", companiesDtoTask.Result.Error);
                return Result<CompanyError>.Fail<Page<Company>>(companiesDtoTask.Result.Error!);
            }

            if (companiesDtoTask.Result.Value.TotalCount <= 0)
            {
                _logger.LogInformation("No companies found.");
            }

            if (exchangeLookupTask.Result.Failed)
            {
                return Result<CompanyError>.Fail<Page<Company>>(exchangeLookupTask.Result.Error!);
            }

            return CreateFromCompanyDtoPage(
                companiesDtoTask.Result.Value,
                exchangeLookupTask.Result.Value);
        }

        public async Task<Result<CompanyError, Company>> UpdateCompanyAsync(
            Guid id, UpdateCompanyRequest updateRequest, CancellationToken cancellationToken)
        {
            id.ThrowIfEmpty();
            updateRequest.ThrowIfNull();

            _logger.LogInformation("Updating company {CompanyId} with new values: {UpdateRequest}", id, updateRequest);

            var existingCompanyTask = GetCompanyByIdAsync(id, cancellationToken);
            var exchangeLookupTask = GetStockExchangeLookupByNameAsync(cancellationToken);

            await Task.WhenAll(existingCompanyTask, exchangeLookupTask);

            if (existingCompanyTask.Result.Failed)
            {
                return Result<CompanyError>.Fail<Company>(existingCompanyTask.Result.Error!);
            }

            if (exchangeLookupTask.Result.Failed)
            {
                return Result<CompanyError>.Fail<Company>(exchangeLookupTask.Result.Error!);
            }

            var existingCompany = existingCompanyTask.Result.Value;
            var updatedCompanyResult = existingCompany.Update(updateRequest, exchangeLookupTask.Result.Value);
            if (updatedCompanyResult.Failed)
            {
                _logger.LogWarning("Failed to update company {CompanyId} due to validation error: {Error}", id, updatedCompanyResult.Error);
                return updatedCompanyResult;
            }

            var updateResult = await _companyRepository.UpdateAsync(updatedCompanyResult.Value, cancellationToken);
            if (updateResult.Failed)
            {
                _logger.LogError("Failed to update company {CompanyId}. Error: {Error}", id, updateResult.Error);
                return Result<CompanyError>.Fail<Company>(updateResult.Error!);
            }

            _logger.LogInformation("Successfully updated company {CompanyId}.", id);

            return updatedCompanyResult;
        }

        private async Task<Result<CompanyError, IReadOnlyDictionary<string, string>>> GetStockExchangeLookupByNameAsync(CancellationToken cancellationToken)
        {
            var exchangeResult = await _stockExchangeProvider.GetStockExchangesAsync(cancellationToken);

            if (exchangeResult.Failed)
            {
                _logger.LogError("Failed to retrieve stock exchange data: {Error}", exchangeResult.Error);
                return Result<CompanyError>.Fail<IReadOnlyDictionary<string, string>>(exchangeResult.Error!);
            }

            var exchangeLookupByName = exchangeResult.Value
                .ToDictionary(e => e.ExchangeName, e => e.MicCode, StringComparer.OrdinalIgnoreCase);

            return Result<CompanyError>.Ok((IReadOnlyDictionary<string, string>)exchangeLookupByName);
        }

        private async Task<Result<CompanyError, IReadOnlyDictionary<string, string>>> GetStockExchangeLookupByMicCodeAsync(CancellationToken cancellationToken)
        {
            var exchangeResult = await _stockExchangeProvider.GetStockExchangesAsync(cancellationToken);

            if (exchangeResult.Failed)
            {
                _logger.LogError("Failed to retrieve stock exchange data: {Error}", exchangeResult.Error);
                return Result<CompanyError>.Fail<IReadOnlyDictionary<string, string>>(exchangeResult.Error!);
            }

            var exchangeLookupByName = exchangeResult.Value
                .ToDictionary(e => e.MicCode, e => e.ExchangeName, StringComparer.OrdinalIgnoreCase);

            return Result<CompanyError>.Ok((IReadOnlyDictionary<string, string>)exchangeLookupByName);
        }

        private async Task<Result<CompanyError, bool>> ExistsByIsinAsync(string isin, CancellationToken cancellationToken)
        {
            var existingCompanyResult = await _companyRepository.ExistsByIsinAsync(isin, cancellationToken);

            if (existingCompanyResult.Failed)
            {
                return Result<CompanyError>.Fail<bool>(CompanyError.SomethingWentWrong);
            }

            if (existingCompanyResult.Value)
            {
                _logger.LogWarning("Company with ISIN {Isin} already exists", isin);
                return Result<CompanyError>.Fail<bool>(CompanyError.DuplicateIsin);
            }

            return existingCompanyResult;
        }

        private Result<CompanyError, Page<Company>> CreateFromCompanyDtoPage(
            Page<CompanyDto> page, IReadOnlyDictionary<string, string> exchangeLookupByMic)
        {
            var companyEdgesResult = page.Items.Select(companyDto => Company.Create(companyDto, exchangeLookupByMic)).ToList();

            if (companyEdgesResult.Any(companyResult => companyResult.Failed))
            {
                return Result<CompanyError>.Fail<Page<Company>>(companyEdgesResult.First(c => c.Failed).Error!);
            }

            return Result<CompanyError>.Ok(new Page<Company>(
                items: companyEdgesResult.Select(c => c.Value).ToList(),
                totalCount: page.TotalCount,
                pageInfo: page.PageInfo));
        }
    }
}
