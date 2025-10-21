using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.UserRoles.Entity;
using VirtualPark.BusinessLogic.Users.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("Users");

        entity.HasKey(u => u.Id);
        entity.Property(u => u.Id).ValueGeneratedNever();

        entity.Property(u => u.Name).IsRequired();
        entity.Property(u => u.LastName).IsRequired();
        entity.Property(u => u.Email).IsRequired();
        entity.Property(u => u.Password).IsRequired();

        entity.HasIndex(u => u.Email).IsUnique();

        entity
            .HasOne(u => u.VisitorProfile)
            .WithMany()
            .HasForeignKey(u => u.VisitorProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        entity
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRole>(
                j =>
                    j.HasOne<Role>()
                        .WithMany()
                        .HasForeignKey(ur => ur.RoleId)
                        .OnDelete(DeleteBehavior.Restrict),
                j =>
                    j.HasOne<User>()
                        .WithMany()
                        .HasForeignKey(ur => ur.UserId)
                        .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.ToTable("UserRoles");
                    j.HasKey(ur => new { ur.UserId, ur.RoleId });
                });
    }
}
