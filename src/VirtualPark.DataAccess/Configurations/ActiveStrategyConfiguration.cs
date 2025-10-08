using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.Strategy.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class ActiveStrategyConfiguration : IEntityTypeConfiguration<ActiveStrategy>
{
    public void Configure(EntityTypeBuilder<ActiveStrategy> entity)
    {
        entity.ToTable("ActiveStrategies");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.StrategyKey)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(e => e.Date)
            .IsRequired()
            .HasColumnType("date")
            .HasConversion(
                d => d.ToDateTime(TimeOnly.MinValue),
                dt => DateOnly.FromDateTime(dt));

        entity.HasIndex(e => e.Date)
            .IsUnique();
    }
}
