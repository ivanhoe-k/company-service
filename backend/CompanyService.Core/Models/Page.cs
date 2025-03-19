using System.Collections.Generic;
using CompanyService.Core.Common;

namespace CompanyService.Core.Models
{
    public sealed record Page<TModel>
    {
        public Page(
            IReadOnlyCollection<Edge<TModel>> edges,
            int totalCount,
            PageInfo pageInfo)
        {
            edges.ThrowIfNull();
            pageInfo.ThrowIfNull();

            Edges = edges;
            TotalCount = totalCount;
            PageInfo = pageInfo;
        }

        public IReadOnlyCollection<Edge<TModel>> Edges { get; }

        public int TotalCount { get; }

        public PageInfo PageInfo { get; }
    }
}
