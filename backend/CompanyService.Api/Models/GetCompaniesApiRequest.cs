using System.ComponentModel.DataAnnotations;
using CompanyService.Domain.Models;

namespace CompanyService.Api.Models
{
    public sealed record GetCompaniesApiRequest(
        SortOrder SortOrder, 
        CompanyApiFilter? Filter,

        [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be at least 1.")]
        int PageNumber = 1,

        [Range(
            GetCompaniesRequest.MinPageSize,
            GetCompaniesRequest.MaxPageSize,
            ErrorMessage = "Page Size must be between {1} and {100}.")]
        int PageSize = 5);
}
