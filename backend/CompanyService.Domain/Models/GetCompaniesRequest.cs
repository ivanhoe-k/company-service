using CompanyService.Core.Common;

namespace CompanyService.Domain.Models
{
    public sealed record GetCompaniesRequest
    {
        public const int MinLimit = 1;
        public const int MaxLimit = 100;

        public string? Cursor { get; } 

        public int Limit { get; }

        public SortOrder? SortOrder { get; }

        public CompanyFilter? Filter { get; }

        public GetCompaniesRequest(
            string? cursor, 
            SortOrder? sortOrder, 
            CompanyFilter? filter,
            int limit = 10)
        {
            limit.ThrowIf(limit < MinLimit || limit > MaxLimit, $"Limit must be between {MinLimit} and {MaxLimit}.");

            Cursor = cursor;
            Limit = limit;
            SortOrder = sortOrder;
            Filter = filter;
        }
    }
}
