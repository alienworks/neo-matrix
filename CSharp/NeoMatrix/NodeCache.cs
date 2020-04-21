using System.Collections.Concurrent;
using NeoMatrix.Validation;

namespace NeoMatrix
{
    public sealed class NodeCache
    {
        public ConcurrentDictionary<string, ValidateResult<bool>> MethodsResult { get; } = new ConcurrentDictionary<string, ValidateResult<bool>>();
    }
}