using System.Collections.Generic;
using CompanyService.Core.Common;

namespace CompanyService.Core.Models
{
    public sealed record Page<TModel>
    {
        public Page(
            IReadOnlyCollection<TModel> items,
            int totalCount,
            PageInfo pageInfo)
        {
            items.ThrowIfNull();
            pageInfo.ThrowIfNull();

            Items = items;
            TotalCount = totalCount;
            PageInfo = pageInfo;
        }

        public IReadOnlyCollection<TModel> Items { get; }

        public int TotalCount { get; }

        public PageInfo PageInfo { get; }
    }
}
