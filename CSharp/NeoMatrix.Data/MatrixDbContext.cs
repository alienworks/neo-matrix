using Microsoft.EntityFrameworkCore;
using NeoMatrix.Data.Models;

namespace NeoMatrix.Data
{
    public sealed class MatrixDbContext : DbContext
    {
        public MatrixDbContext(DbContextOptions<MatrixDbContext> options) : base(options)
        {
        }

        public DbSet<MatrixItemEntity> MatrixItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MatrixItemEntity>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Id).ValueGeneratedOnAdd();
                e.HasIndex(p => p.GroupId);
                e.Property(p => p.GroupId).IsRequired();
                e.HasIndex(p => p.Url);
                e.Property(p => p.Url).IsRequired();
                e.HasIndex(p => p.Method);
                e.Property(p => p.Method).IsRequired();
                e.Property(p => p.CreateTime)
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("now()")
                .IsRequired();
            });
        }
    }
}