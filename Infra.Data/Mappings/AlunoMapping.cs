using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Mappings;

public class AlunoMapping : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> entity)
    {
        var tableName = nameof(Aluno);

        entity.ToTable(tableName);

        entity.HasKey(a => a.Id);

        entity.Property(a => a.Id).HasColumnName($"{tableName}Id");
        entity.Property(a => a.Name).IsRequired();
        entity.Property(a => a.CPF).IsRequired().HasMaxLength(11);
        entity.Property(a => a.CreateUser).IsRequired();
        entity.Property(a => a.CreateDate).IsRequired();
        entity.Property(a => a.UpdateUser).IsRequired(false);
        entity.Property(a => a.UpdateDate);
    }
}
