using System.ComponentModel.DataAnnotations;
using CompanyService.Domain.Models;

namespace CompanyService.Api.Models
{
    public sealed record GetCompaniesApiRequest(
        [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be at least 1.")]
        int PageNumber,

        [Range(
            GetCompaniesRequest.MinPageSize,
            GetCompaniesRequest.MaxPageSize, 
            ErrorMessage = "Page Size must be between {1} and {100}.")]
        int PageSize,

        SortOrder SortOrder, 
        
        CompanyApiFilter? Filter);
}
