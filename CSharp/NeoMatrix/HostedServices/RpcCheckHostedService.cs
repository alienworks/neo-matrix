using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NeoMatrix.Data.Models;

namespace NeoMatrix.HostedServices
{
    internal sealed class RpcCheckHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        private readonly NodeCaller _caller;

        private readonly ConcurrentDictionary<string, NodeCache> _cache = new ConcurrentDictionary<string, NodeCache>();

        public RpcCheckHostedService(IConfiguration configuration,
            ILogger<RpcCheckHostedService> logger,
            NodeCaller caller)
        {
            _configuration = configuration;
            _logger = logger;

            _caller = caller;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var node = new Node() { Url = "http://seed1.ngd.network:20332" };
            var nodeCache = await _caller.ExecuteAsync(node);
            _cache.TryAdd(node.Url, nodeCache);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}