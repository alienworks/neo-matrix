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
    public sealed class MatrixCache : IMatrixCache
    {
        private static ConcurrentDictionary<string, Node> nodeCache;
        private static ConcurrentDictionary<string, ValidationResult> validations;
        private static NodePlugin[] nodePluginCache;

        private readonly MatrixDbContext db;

        public MatrixCache(MatrixDbContext db)
        {
            this.db = db;

            if (nodeCache == null)
            {
                nodeCache = new ConcurrentDictionary<string, Node>(db.Nodes.ToDictionary(n => n.Url));
            }

            if (validations == null)
            {
                validations = new ConcurrentDictionary<string, ValidationResult>(
                    db.ValidationResults.ToDictionary(n => n.Name)
                );
            }
            if (nodePluginCache == null)
            {
                nodePluginCache = db.NodePlugins.ToArray();
            }
        }


        public async Task<NodePlugin> CreateNodePluginAsync(NodePlugin nodePlugin)
        {
            EntityEntry<NodePlugin> added = await db.NodePlugins.AddAsync(nodePlugin);
            int affected = await db.SaveChangesAsync();

            if (affected == 1)
            {
                nodePluginCache.Append(nodePlugin);
                return nodePlugin;
            }
            return null;
        }

        public async Task<ValidationResult> CreateValidationAsync(ValidationResult validationResult)
        {
            await db.ValidationResults.AddAsync(validationResult);
            int affected = await db.SaveChangesAsync();

            if (affected == 1)
            {
                return validations.AddOrUpdate(validationResult.Url, validationResult, UpdateValidationCache);
            }
            return null;
        }

        private ValidationResult UpdateValidationCache(string url, ValidationResult validationResult)
        {
            if (validations.TryGetValue(url, out ValidationResult old))
                if (validations.TryUpdate(url, validationResult, old)) return validationResult;
            return null;
        }
        public async Task<Node> CreateNodeAsync(Node node)
        {
            EntityEntry<Node> added = await db.Nodes.AddAsync(node);
            int affected = await db.SaveChangesAsync();

            if (affected == 1)
            {
                return nodeCache.AddOrUpdate(node.Url, node, UpdateNodeCache);
            }
            return null;
        }

        private Node UpdateNodeCache(string url, Node node)
        {
            if (nodeCache.TryGetValue(url, out Node old))
                if (nodeCache.TryUpdate(url, node, old)) return node;
            return null;
        }
    }
}