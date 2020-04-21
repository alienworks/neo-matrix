using NeoMatrix.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NeoMatrix.Caches
{
    public interface IMatrixCache
    {
        Task<Node> CreateNodeAsync(Node node);
        Task<NodePlugin> CreateNodePluginAsync(NodePlugin nodePlugin);
        Task<ValidationResult> CreateValidationAsync(ValidationResult validationResult);
    }
}
