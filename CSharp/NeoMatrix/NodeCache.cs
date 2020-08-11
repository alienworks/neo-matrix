using System.Collections.Concurrent;
using NeoMatrix.Data.Models;
using NeoMatrix.Validation;

namespace NeoMatrix
{
    public sealed class NodeCache
    {
        public NodeCache(Node node)
        {
            Node = node;
        }

        public Node Node { get; }

        public ConcurrentDictionary<string, ValidateResult<ValidationResultType>> MethodsResult { get; } = new ConcurrentDictionary<string, ValidateResult<ValidationResultType>>();
    }
}