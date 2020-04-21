using Microsoft.EntityFrameworkCore;
using NeoMatrix.Data.Models;
using System;

namespace NeoMatrix.Data
{
    public class MatrixDbContext : DbContext
    {
        public DbSet<Node> Nodes { get; set; }
        public DbSet<ValidationResult> ValidationResults { get; set; }
        public DbSet<NodePlugin> NodePlugins { get; set; }

        public MatrixDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}