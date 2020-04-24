using System;
using System.Text.Json;

namespace NeoMatrix.Rpc.Http
{
    public sealed class RpcHttpClientConfig
    {
        public Version ApiVersion { get; set; }

        public JsonSerializerOptions JsonSerializerOptions { get; set; }
    }
}