using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.Tickets.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> entity)
    {
        entity.ToTable("Tickets");

        entity.HasKey(t => t.Id);
        entity.Property(t => t.Id).ValueGeneratedNever();

        entity.Property(t => t.Date).HasColumnType("datetime2");
        entity.Property(t => t.Type).IsRequired();
        entity.Property(t => t.QrId).IsRequired();

        entity
            .HasOne(t => t.Event)
            .WithMany(e => e.Tickets)
            .HasForeignKey(t => t.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        entity
            .HasOne(t => t.Visitor)
            .WithMany()
            .HasForeignKey(t => t.VisitorProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(t => t.EventId);
        entity.HasIndex(t => t.VisitorProfileId);
        entity.HasIndex(t => t.QrId).IsUnique();
    }
}
