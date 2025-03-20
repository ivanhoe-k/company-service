using System;
using System.Threading;
using System.Threading.Tasks;
using CompanyService.Api.Mappers;
using CompanyService.Api.Models;
using CompanyService.Api.Utils;
using CompanyService.Core.Common;
using CompanyService.Core.Validation.Attributes;
using CompanyService.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyService.Api.Controllers;

[Route(ApiConstants.CompanyEndpoint)]
public class CompanyController : CompanyControllerBase
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
}
