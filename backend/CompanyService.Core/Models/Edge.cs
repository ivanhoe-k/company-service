using CompanyService.Core.Common;

namespace CompanyService.Core.Models
{
    public sealed record Edge<T>
    {
        public Edge(T node, string cursor)
        {
            node.ThrowIfNull();
            cursor.ThrowIfNullOrWhiteSpace();

            Node = node;
            Cursor = cursor;
        }

        public T Node { get; }

        public string Cursor { get; }
    }
}
