using NeoMatrix.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NeoMatrix.Caches
{
    public interface INodeCache
    {
        Task<Node> CreateAsync(Node node);

    }
}
