using System.Collections.Generic;

namespace NeoMatrix.Configuration
{
    public sealed class RpcMethodOptions
    {
        public HashSet<int> Indexes { get; set; }

        public RpcMethodOption[] Items { get; set; }
    }
}