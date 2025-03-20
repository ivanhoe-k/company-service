using System.Collections.Generic;
using System.Linq;
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

        public Page(
           IReadOnlyCollection<TModel> items,
           int totalCount,
           PageInfo pageInfo)
        {
            items.ThrowIfNull();
            pageInfo.ThrowIfNull();

            Edges = items.Select((edge, index) => new Edge<TModel>(edge, index.ToString())).ToList();
            TotalCount = totalCount;
            PageInfo = pageInfo;
        }

        public IReadOnlyCollection<TModel> Items => Edges.Select(edge => edge.Node).ToList();

        public IReadOnlyCollection<Edge<TModel>> Edges { get; }

        public int TotalCount { get; }

        public PageInfo PageInfo { get; }
    }
}
