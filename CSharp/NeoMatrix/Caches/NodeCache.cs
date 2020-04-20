using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NeoMatrix.Data;
using NeoMatrix.Data.Models;
using NeoMatrix.Rpc;
using NeoMatrix.Validation;

namespace NeoMatrix.Caches
{
    public sealed class NodeCache : INodeCache
    {
        private static ConcurrentDictionary<string, Node> nodeCache;

        private readonly MatrixDbContext db;

        public NodeCache(MatrixDbContext db)
        {
            this.db = db;

            if (nodeCache == null)
            {
                nodeCache = new ConcurrentDictionary<string, Node>(db.Nodes.ToDictionary(n => n.Url));
            }
        }

        public async Task<Node> CreateAsync(Node node)
        {
            EntityEntry<Node> added = await db.Nodes.AddAsync(node);
            int affected = await db.SaveChangesAsync();

            if (affected == 1)
            {
                return nodeCache.AddOrUpdate(node.Url, node, UpdateCache);
            }
            return null;
        }

        private Node UpdateCache(string url, Node node)
        {
            if (nodeCache.TryGetValue(url, out Node old))
                if (nodeCache.TryUpdate(url, node, old)) return node;
            return null;
        }
    }
}