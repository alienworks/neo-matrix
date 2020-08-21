using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NeoMatrix.Data;
using NeoMatrix.Data.Models;

namespace NeoMatrix.HostedServices
{
    internal sealed class RpcCheckHostedService : IHostedService
    {
        private readonly ILogger _logger;

        private readonly NodeSeedsLoader _seedsLoader;
        private readonly NodeCaller _caller;

        private readonly MatrixDbContext _dbContext;

        private readonly ConcurrentDictionary<int, NodeCache> _cache = new ConcurrentDictionary<int, NodeCache>();

        public RpcCheckHostedService(
            ILogger<RpcCheckHostedService> logger,
            NodeSeedsLoader seedsLoader,
            NodeCaller caller,
            MatrixDbContext dbContext)
        {
            _logger = logger;

            _seedsLoader = seedsLoader;
            _caller = caller;

            _dbContext = dbContext;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var nodes = _seedsLoader.Load();
            if (nodes is null || nodes.Count < 1)
            {
                return;
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Id = i + 1;
            }
#if DEBUG
            Stopwatch sw = Stopwatch.StartNew();
#endif
            foreach (var node in nodes)
            {
                var nodeCache = await _caller.ExecuteAsync(node);
                _cache.TryAdd(node.Id, nodeCache);
            }
#if DEBUG
            sw.Stop();
            _logger.LogInformation("Use time: {0}", sw.Elapsed.ToString());
#endif
            long groupId = CreateGroupId();
            var entities = new List<MatrixItemEntity>(_cache.Count * 32);
            foreach (var cache in _cache.Values)
            {
                var node = cache.Node;
                foreach (var methodItem in cache.MethodsResult)
                {
                    entities.Add(new MatrixItemEntity()
                    {
                        Net = node.Net,
                        Url = node.Url,
                        Method = methodItem.Key,
                        Available = (byte)methodItem.Value.Result,
                        GroupId = groupId,
                        Error = methodItem.Value.ToFullErrorMsg()
                    });
                }
            }
            await _dbContext.MatrixItems.AddRangeAsync(entities, cancellationToken);
            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            long deletedMaxId = await CheckDeleteOldestGroupAsync(groupId, cancellationToken);
            if (deletedMaxId >= 1000_000_000)
            {
                await _dbContext.Database.ExecuteSqlRawAsync($"ALTER TABLE `{nameof(_dbContext.MatrixItems)}` AUTO_INCREMENT = 1;", cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private static long CreateGroupId()
        {
            DateTime now = DateTime.Now;
            return now.Year * 10000000000L + now.Month * 100000000 + now.Day * 1000000 + now.Hour * 10000 + now.Minute * 100 + now.Second;
        }

        private async Task<long> CheckDeleteOldestGroupAsync(long currentGroupId, CancellationToken token)
        {
            long minGroupId = await _dbContext.MatrixItems.MinAsync(p => p.GroupId, token);
            if (minGroupId != currentGroupId)
            {
                var oldItems = await _dbContext.MatrixItems.Where(p => p.GroupId == minGroupId).ToArrayAsync(token);
                if (oldItems.Length > 0)
                {
                    _dbContext.MatrixItems.RemoveRange(oldItems);
                    await _dbContext.SaveChangesAsync(token);
                    return oldItems.Max(e => e.Id);
                }
            }
            return 0;
        }
    }
}