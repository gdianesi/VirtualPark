using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.AttractionsEvents.Entity;
using VirtualPark.BusinessLogic.Events.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> entity)
    {
        entity.ToTable("Events");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.Name).IsRequired();
        entity.Property(e => e.Date).HasColumnType("datetime2");
        entity.Property(e => e.Capacity).IsRequired();
        entity.Property(e => e.Cost).IsRequired();

        entity
            .HasMany(e => e.Attractions)
            .WithMany(a => a.Events)
            .UsingEntity<AttractionEvent>(
                j => j
                    .HasOne<Attraction>()
                    .WithMany()
                    .HasForeignKey(ae => ae.AttractionId)
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne<Event>()
                    .WithMany()
                    .HasForeignKey(ae => ae.EventId)
                    .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.ToTable("AttractionEvents");
                    j.HasKey(ae => new { ae.EventId, ae.AttractionId });
                    j.HasIndex(ae => ae.AttractionId);
                    j.HasIndex(ae => ae.EventId);
                });
    }
}
