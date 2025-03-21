using CompanyService.Core.Common;
using CompanyService.Core.Models;
using CompanyService.Domain.Errors;

namespace CompanyService.Domain.Models;

public sealed record GetCompaniesRequest
{
    public const int MinPageSize = 1;
    public const int MaxPageSize = 100;

    public int PageNumber { get; }

    public int PageSize { get; }

    public SortOrder SortOrder { get; }

    public CompanyFilter? Filter { get; }

    private GetCompaniesRequest(
        int pageNumber,
        int pageSize,
        CompanyFilter? filter,
        SortOrder sortOrder = SortOrder.Asc)
    {
        pageNumber.ThrowIf(pageNumber < 1, "PageNumber must be at least 1.");
        pageSize.ThrowIf(pageSize < MinPageSize || pageSize > MaxPageSize, $"PageSize must be between {MinPageSize} and {MaxPageSize}.");

        PageNumber = pageNumber;
        PageSize = pageSize;
        SortOrder = sortOrder;
        Filter = filter;
    }

    public static Result<CompanyError, GetCompaniesRequest> Create(
        int pageNumber = 1,
        int pageSize = 10,
        CompanyFilter? filter = default,
        SortOrder sortOrder = SortOrder.Asc)
    {
        return Result<CompanyError>.Ok(
            new GetCompaniesRequest(pageNumber, pageSize, filter, sortOrder));
    }
}
