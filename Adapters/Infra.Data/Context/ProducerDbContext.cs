using Domain.Entities;
using Domain.Entities.Base;
using Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Context;

public class ProducerDbContext : DbContext
{
    public ProducerDbContext(DbContextOptions<ProducerDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Category { get; set; }
    public DbSet<Product> Product { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new CategoryMapping());
        builder.ApplyConfiguration(new ProductMapping());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is EntityBase && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property("CreateDate").IsModified = false;
                entityEntry.Property("CreateUser").IsModified = false;
                ((EntityBase)entityEntry.Entity).UpdateDate = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
