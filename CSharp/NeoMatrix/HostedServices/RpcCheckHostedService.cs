using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NeoMatrix.Caches;
using NeoMatrix.Data.Models;

namespace NeoMatrix.HostedServices
{
    internal sealed class RpcCheckHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        private readonly NodeCaller _caller;
        private readonly IMatrixCache _matrixCache;

        public RpcCheckHostedService(
            IConfiguration configuration,
            ILogger<RpcCheckHostedService> logger,
            NodeCaller caller,
            IMatrixCache matrixCache)
        {
            _configuration = configuration;
            _logger = logger;

            _caller = caller;
            _matrixCache = matrixCache;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var node = new Node() { Url = "http://seed1.ngd.network:20332" };
            // await _caller.ExecuteAsync(node);
            // await _nodeCache.CreateAsync(node);
            NodePlugin[] nodePlugins = await _caller.GetPluginsAsync(node);
            
            foreach (var nodePlugin in nodePlugins)
            {
                await _matrixCache.CreateNodePluginAsync(nodePlugin);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}