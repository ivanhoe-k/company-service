namespace CompanyService.Core.Models
{
    public record PageInfo(string StartCursor, string EndCursor, bool HasNextPage, bool HasPreviousPage);
}
