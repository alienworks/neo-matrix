using System;
using System.Text.Json;
using NeoMatrix.Rpc;
using NeoMatrix.Rpc.Http;
using NeoMonitor.Rpc.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static INeoRpcServiceBuilder AddNeoRpcHttpClient(this IServiceCollection services, Action<RpcHttpClientConfig> configure = null)
        {
            var config = new RpcHttpClientConfig()
            {
                ApiVersion = new Version(2, 0),
                JsonSerializerOptions = new JsonSerializerOptions()
                {
                    AllowTrailingCommas = true,
                    PropertyNamingPolicy = new LowCaseJsonNamingPolicy()
                }
            };
            configure?.Invoke(config);
            services
                .AddSingleton(config)
                .AddHttpClient<RpcHttpClient>();
            return new NeoRpcServiceBuilder(services);
        }
    }
}