using CompanyService.Core.Common;
using CompanyService.Core.Models;
using CompanyService.Domain.Errors;

namespace CompanyService.Domain.Models
{
    public sealed record GetCompaniesRequest
    {
        public const int MinLimit = 1;
        public const int MaxLimit = 100;

        public string? Cursor { get; } 

        public int Limit { get; }

        public SortOrder SortOrder { get; }

        public CompanyFilter? Filter { get; }

        private GetCompaniesRequest(
            string? cursor, 
            CompanyFilter? filter,
            SortOrder sortOrder = SortOrder.Asc,
            int limit = 10)
        {
            limit.ThrowIf(limit < MinLimit || limit > MaxLimit, $"Limit must be between {MinLimit} and {MaxLimit}.");

            Cursor = cursor;
            Limit = limit;
            SortOrder = sortOrder;
            Filter = filter;
        }

        public static Result<CompanyError, GetCompaniesRequest> Create(
            string? cursor = default,
            CompanyFilter? filter = default,
            SortOrder sortOrder = SortOrder.Asc,
            int limit = 10)
        {
            return Result<CompanyError>.Ok(
                new GetCompaniesRequest(cursor, filter, sortOrder, limit));
        }
    }
}
