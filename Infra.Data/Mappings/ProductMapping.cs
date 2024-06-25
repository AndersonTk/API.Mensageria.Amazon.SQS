using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entity)
    {
        var tableName = nameof(Product);

        entity.ToTable(tableName);

        entity.HasKey(a => a.Id);

        entity.Property(a => a.Id).HasColumnName($"{tableName}Id");
        entity.Property(a => a.Name).IsRequired().HasMaxLength(255);
        entity.Property(a => a.CreateUser).IsRequired();
        entity.Property(a => a.CreateDate).IsRequired();
        entity.Property(a => a.UpdateUser).IsRequired(false);
        entity.Property(a => a.UpdateDate).IsRequired(false);
    }
}
