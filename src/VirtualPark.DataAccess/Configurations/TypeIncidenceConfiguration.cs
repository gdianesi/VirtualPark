using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class TypeIncidenceConfiguration : IEntityTypeConfiguration<TypeIncidence>
{
    public void Configure(EntityTypeBuilder<TypeIncidence> entity)
    {
        entity.ToTable("TypeIncidences");

        entity.HasKey(t => t.Id);
        entity.Property(t => t.Id).ValueGeneratedNever();

        entity.Property(t => t.Type).IsRequired();

        entity.HasIndex(t => t.Type).IsUnique();
    }
}
