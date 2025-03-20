using System;
using System.Threading;
using System.Threading.Tasks;
using CompanyService.Core.Common;
using CompanyService.Domain.Errors;
using CompanyService.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyService.Api.Controllers;

[ApiController]
[Route("api/companies")]
public class CompanyController : ControllerBase
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

    /// <summary>
    /// Retrieves a company by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the company.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A company if found, otherwise an error response.</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCompanyById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving company by ID: {CompanyId}", id);

        if (id == Guid.Empty)
        {
            return BadRequest("Invalid company ID.");
        }

        var result = await _companyManagerService.GetCompanyByIdAsync(id, cancellationToken);

        if (result.Failed)
        {
            return result.Error!.Code switch
            {
                CompanyErrorCode.NotFound => NotFound("Company not found."),
                _ => StatusCode(500, "An error occurred while retrieving the company.")
            };
        }

        return Ok(result.Value);
    }
}
