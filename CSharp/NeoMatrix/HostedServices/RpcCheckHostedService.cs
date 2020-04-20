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
        private readonly INodeCache _nodeCache;

        public RpcCheckHostedService(
            IConfiguration configuration,
            ILogger<RpcCheckHostedService> logger,
            NodeCaller caller,
            INodeCache nodeCache)
        {
            _configuration = configuration;
            _logger = logger;

            _caller = caller;
            _nodeCache = nodeCache;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var node = new Node() { Url = "http://seed1.ngd.network:20332" };
            await _caller.ExecuteAsync(node);
            await _nodeCache.CreateAsync(node);

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}