using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.Rankings.Entity;
using VirtualPark.BusinessLogic.Users.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class RankingConfiguration : IEntityTypeConfiguration<Ranking>
{
    public void Configure(EntityTypeBuilder<Ranking> entity)
    {
        entity.ToTable("Rankings");

        entity.HasKey(r => r.Id);
        entity.Property(r => r.Id).ValueGeneratedNever();

        entity.Property(r => r.Date).HasColumnType("datetime2");
        entity.Property(r => r.Period).IsRequired();

        entity
            .HasMany(r => r.Entries)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "RankingUsers",
                j => j.HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<Ranking>()
                    .WithMany()
                    .HasForeignKey("RankingId")
                    .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.ToTable("RankingUsers");
                    j.HasKey("RankingId", "UserId");
                    j.HasIndex("UserId");
                    j.HasIndex("RankingId");
                });
    }
}
