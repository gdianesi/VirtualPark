using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.ClocksApp.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class ClockAppConfiguration : IEntityTypeConfiguration<ClockApp>
{
    public void Configure(EntityTypeBuilder<ClockApp> entity)
    {
        entity.ToTable("ClockApp");

        entity.HasKey(c => c.Id);
        entity.Property(c => c.Id).ValueGeneratedNever();

        entity.Property(c => c.DateSystem)
            .HasColumnType("datetime2")
            .IsRequired();

        entity.HasIndex(c => c.Id).IsUnique();
    }
}
