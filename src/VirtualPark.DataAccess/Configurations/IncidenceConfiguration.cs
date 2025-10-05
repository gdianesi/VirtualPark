using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.Incidences.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class IncidenceConfiguration : IEntityTypeConfiguration<Incidence>
{
    public void Configure(EntityTypeBuilder<Incidence> entity)
    {
        entity.ToTable("Incidences");

        entity.HasKey(i => i.Id);
        entity.Property(i => i.Id).ValueGeneratedNever();

        entity.Property(i => i.Description).IsRequired();
        entity.Property(i => i.Start).HasColumnType("datetime2");
        entity.Property(i => i.End).HasColumnType("datetime2");
        entity.Property(i => i.Active).HasDefaultValue(true);

        entity
            .HasOne(i => i.Type)
            .WithMany()
            .HasForeignKey(i => i.TypeIncidenceId)
            .OnDelete(DeleteBehavior.Restrict);

        entity
            .HasOne(i => i.Attraction)
            .WithMany()
            .HasForeignKey(i => i.AttractionId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(i => i.TypeIncidenceId);
        entity.HasIndex(i => i.AttractionId);
    }
}
