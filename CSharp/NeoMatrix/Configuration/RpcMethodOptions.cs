using System;
using System.Collections.Generic;
using System.Text;

namespace NeoMatrix.Configuration
{
    public sealed class RpcMethodOptions
    {
        public CommonMethodOption Common { get; set; }
        public RpcMethodOption[] Items { get; set; }
    }
}
