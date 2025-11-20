using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.VisitsScore.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class VisitScoreConfiguration : IEntityTypeConfiguration<VisitScore>
{
    public void Configure(EntityTypeBuilder<VisitScore> entity)
    {
        entity.ToTable("VisitScores");

        entity.HasKey(vs => vs.Id);
        entity.Property(vs => vs.Id).ValueGeneratedNever();

        entity.Property(vs => vs.Origin)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(vs => vs.OccurredAt)
            .HasColumnType("datetime2")
            .IsRequired();

        entity.Property(vs => vs.Points)
            .IsRequired();

        entity.Property(vs => vs.DayStrategyName)
            .HasMaxLength(100)
            .IsRequired(false);

        entity.Property(vs => vs.VisitRegistrationId)
            .IsRequired();

        entity.HasOne(vs => vs.VisitRegistration)
            .WithMany(vr => vr.ScoreEvents)
            .HasForeignKey(vs => vs.VisitRegistrationId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(vs => vs.VisitRegistrationId);
        entity.HasIndex(vs => vs.OccurredAt);
        entity.HasIndex(vs => vs.Origin);
    }
}
