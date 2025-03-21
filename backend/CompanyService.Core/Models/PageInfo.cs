namespace CompanyService.Core.Models
{
    using CompanyService.Core.Common;

    public sealed record PageInfo
    {
        public int CurrentPage { get; }

        public int PageSize { get; }

        public int TotalPages { get; }

        public bool HasNextPage { get; }

        public bool HasPreviousPage { get; }

        public PageInfo(int currentPage, int pageSize, int totalPages, bool hasNextPage, bool hasPreviousPage)
        {
            currentPage.ThrowIf(currentPage < 1, "CurrentPage must be at least 1.");
            pageSize.ThrowIf(pageSize < 1, "PageSize must be at least 1.");
            totalPages.ThrowIf(totalPages < 0, "TotalPages cannot be negative.");

            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            HasNextPage = hasNextPage;
            HasPreviousPage = hasPreviousPage;
        }
    }
}
