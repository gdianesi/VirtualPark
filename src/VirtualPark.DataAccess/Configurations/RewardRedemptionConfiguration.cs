using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.RewardRedemptions.Entity;
using VirtualPark.BusinessLogic.Rewards.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class RewardRedemptionConfiguration : IEntityTypeConfiguration<RewardRedemption>
{
    public void Configure(EntityTypeBuilder<RewardRedemption> entity)
    {
        entity.ToTable("RewardRedemptions");

        entity.HasKey(r => r.Id);
        entity.Property(r => r.Id).ValueGeneratedNever();

        entity.Property(r => r.Date)
            .IsRequired()
            .HasColumnType("date")
            .HasConversion(
                d => d.ToDateTime(TimeOnly.MinValue),
                dt => DateOnly.FromDateTime(dt));

        entity.Property(r => r.PointsSpent)
            .IsRequired();

        entity
            .HasOne<Reward>()
            .WithMany()
            .HasForeignKey(r => r.RewardId)
            .OnDelete(DeleteBehavior.Restrict);

        entity
            .HasOne<VirtualPark.BusinessLogic.VisitorsProfile.Entity.VisitorProfile>()
            .WithMany()
            .HasForeignKey(r => r.VisitorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
