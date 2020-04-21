using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NeoMatrix.HostedServices
{
    internal sealed class RpcCheckHostedService : IHostedService
    {
        private readonly ILogger _logger;

        private readonly NodeSeedsLoader _seedsLoader;
        private readonly NodeCaller _caller;

        private readonly ConcurrentDictionary<string, NodeCache> _cache = new ConcurrentDictionary<string, NodeCache>();

        public RpcCheckHostedService(
            ILogger<RpcCheckHostedService> logger,
            NodeSeedsLoader seedsLoader,
            NodeCaller caller)
        {
            _logger = logger;

            _seedsLoader = seedsLoader;
            _caller = caller;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var nodes = _seedsLoader.Load();
            if (nodes is null || nodes.Count < 1)
            {
                return;
            }
#if DEBUG
            Stopwatch sw = Stopwatch.StartNew();
#endif
            foreach (var node in nodes)
            {
                var nodeCache = await _caller.ExecuteAsync(node);
                _cache.TryAdd(node.Url, nodeCache);
            }
#if DEBUG
            sw.Stop();
            _logger.LogInformation("Use time: {0}", sw.Elapsed.ToString());
#endif
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}