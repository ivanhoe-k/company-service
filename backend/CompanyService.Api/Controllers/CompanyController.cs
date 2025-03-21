using System;
using System.Threading;
using System.Threading.Tasks;
using CompanyService.Api.Mappers;
using CompanyService.Api.Models;
using CompanyService.Api.Utils;
using CompanyService.Core.Common;
using CompanyService.Core.Models;
using CompanyService.Core.Validation.Attributes;
using CompanyService.Domain.Errors;
using CompanyService.Domain.Models;
using CompanyService.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyService.Api.Controllers
{
    [Authorize]
    [Route(ApiConstants.CompanyEndpoint)]
    public sealed class CompanyController : CompanyControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyManagerService _companyManagerService;

        public CompanyController(
            ILogger<CompanyController> logger,
            ICompanyManagerService companyManagerService)
        {
            logger.ThrowIfNull();
            companyManagerService.ThrowIfNull();

            _logger = logger;
            _companyManagerService = companyManagerService;
        }

        [HttpGet(ApiConstants.CompanyIdRoute)]
        public async Task<ActionResult<CompanyApi>> GetCompanyByIdAsync(
            [NotEmptyGuid] Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving company by ID: {CompanyId}", id);

            var result = await _companyManagerService.GetCompanyByIdAsync(id, cancellationToken);

            return HandleResult(result, DomainToApiMapper.Map);
        }

        [HttpGet(ApiConstants.CompanyByIsinRoute)]
        public async Task<ActionResult<CompanyApi>> GetCompanyByIsinAsync(
            [FromQuery] IsinApi isin, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving company by ISIN: {Isin}", isin.Value);

            var result = await _companyManagerService.GetCompanyByIsinAsync(isin.Value, cancellationToken);

            return HandleResult(result, DomainToApiMapper.Map);
        }

        [HttpGet]
        public async Task<ActionResult<Page<CompanyApi>>> GetCompaniesAsync(
            [FromQuery] GetCompaniesApiRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving companies with request: {@Request}", request);

            var companiesRequestResult = CreateGetCompaniesRequest(request);

            if (companiesRequestResult.Failed)
            {
                return HandleResult(companiesRequestResult);
            }

            var result = await _companyManagerService.GetCompaniesAsync(companiesRequestResult.Value, cancellationToken);

            return HandleResult(result, DomainToApiMapper.Map);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyApi>> CreateCompanyAsync(
            [FromBody] CreateCompanyApiRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating company: {@Request}", request);

            var result = await _companyManagerService.CreateCompanyAsync(ApiToDomainMapper.Map(request), cancellationToken);

            return HandleResult(result, DomainToApiMapper.Map);
        }

        [HttpPut(ApiConstants.CompanyIdRoute)]
        public async Task<ActionResult<CompanyApi>> UpdateCompanyAsync(
            [NotEmptyGuid] Guid id,
            [FromBody] UpdateCompanyApiRequest request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating company {CompanyId}: {@Request}", id, request);

            var result = await _companyManagerService.UpdateCompanyAsync(id, ApiToDomainMapper.Map(request), cancellationToken);

            return HandleResult(result, DomainToApiMapper.Map);
        }

        private Result<CompanyError, GetCompaniesRequest> CreateGetCompaniesRequest(GetCompaniesApiRequest request)
        {
            var filter = default(CompanyFilter);

            if (request.Filter != null)
            {
                var filterResult = CompanyFilter.Create(request.Filter.Name, request.Filter.Exchange, request.Filter.Ticker, request.Filter.Isin?.Value);

                if (filterResult.Failed)
                {
                    return Result<CompanyError>.Fail<GetCompaniesRequest>(filterResult.Error!);
                }

                filter = filterResult.Value;
            }

            return GetCompaniesRequest.Create(
                pageNumber: request.PageNumber,
                filter: filter,
                sortOrder: request.SortOrder,
                pageSize: request.PageSize);
        }
    }
}