using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitsRegistrationsAttractions.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class VisitRegistrationConfiguration : IEntityTypeConfiguration<VisitRegistration>
{
    public void Configure(EntityTypeBuilder<VisitRegistration> entity)
    {
        entity.ToTable("VisitRegistrations");

        entity.HasKey(v => v.Id);
        entity.Property(v => v.Id).ValueGeneratedNever();

        entity.Property(v => v.Date).HasColumnType("datetime2");
        entity.Property(v => v.IsActive).HasDefaultValue(false);

        entity.HasOne(v => v.Visitor)
              .WithMany()
              .HasForeignKey(v => v.VisitorId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(v => v.Ticket)
              .WithMany()
              .HasForeignKey(v => v.TicketId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(v => v.VisitorId);
        entity.HasIndex(v => v.TicketId);

        entity.HasMany(v => v.Attractions)
              .WithMany(a => a.VisitRegistrations)
              .UsingEntity<VisitRegistrationAttraction>(
                  j => j.HasOne<Attraction>()
                        .WithMany()
                        .HasForeignKey(x => x.AttractionId)
                        .OnDelete(DeleteBehavior.Restrict),
                  j => j.HasOne<VisitRegistration>()
                        .WithMany()
                        .HasForeignKey(x => x.VisitRegistrationId)
                        .OnDelete(DeleteBehavior.Restrict),
                  j =>
                  {
                      j.ToTable("VisitRegistrationsAttractions");
                      j.HasKey(x => new { x.AttractionId, x.VisitRegistrationId });
                      j.HasIndex(x => x.AttractionId);
                      j.HasIndex(x => x.VisitRegistrationId);
                  });
    }
}
