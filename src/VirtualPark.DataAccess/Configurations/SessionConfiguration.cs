using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.Sessions.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> entity)
    {
        entity.ToTable("Sessions");

        entity.HasKey(s => s.Id);
        entity.Property(s => s.Id).ValueGeneratedNever();

        entity.Property(s => s.Token).IsRequired();
        entity.HasIndex(s => s.Token).IsUnique();
        entity.Property(s => s.Token).ValueGeneratedNever();

        entity.Property(s => s.UserId).IsRequired();

        entity
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
