using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.Attractions.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class AttractionConfiguration : IEntityTypeConfiguration<Attraction>
{
    public void Configure(EntityTypeBuilder<Attraction> entity)
    {
        entity.ToTable("Attractions");

        entity.HasKey(a => a.Id);
        entity.Property(a => a.Id).ValueGeneratedNever();

        entity.Property(a => a.Type).IsRequired();
        entity.Property(a => a.Name).IsRequired();
        entity.Property(a => a.MiniumAge).IsRequired();
        entity.Property(a => a.Capacity).IsRequired();
        entity.Property(a => a.Description).IsRequired();
        entity.Property(a => a.CurrentVisitors).HasDefaultValue(0);
        entity.Property(a => a.Available).HasDefaultValue(true);
    }
}
