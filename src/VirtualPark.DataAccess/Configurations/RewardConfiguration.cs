using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.Rewards.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class RewardConfiguration : IEntityTypeConfiguration<Reward>
{
    public void Configure(EntityTypeBuilder<Reward> entity)
    {
        entity.ToTable("Rewards");

        entity.HasKey(r => r.Id);
        entity.Property(r => r.Id).ValueGeneratedNever();

        entity.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(250);

        entity.Property(r => r.Cost)
            .IsRequired();

        entity.Property(r => r.QuantityAvailable)
            .IsRequired();

        entity.Property(r => r.RequiredMembershipLevel)
            .IsRequired()
            .HasConversion(
                m => m.ToString(),
                m => Enum.Parse<Membership>(m));
    }
}
