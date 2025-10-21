using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.RolePermissions.Entity;
using VirtualPark.BusinessLogic.Roles.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> entity)
    {
        entity.ToTable("Roles");

        entity.HasKey(r => r.Id);
        entity.Property(r => r.Id).ValueGeneratedNever();

        entity.Property(r => r.Name).IsRequired();
        entity.Property(r => r.Description).IsRequired();

        entity.HasIndex(r => r.Name).IsUnique();

        entity
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity<RolePermission>(
                j =>
                    j.HasOne<Permission>()
                        .WithMany()
                        .HasForeignKey(rp => rp.PermissionId)
                        .OnDelete(DeleteBehavior.Restrict),
                j =>
                    j.HasOne<Role>()
                        .WithMany()
                        .HasForeignKey(rp => rp.RoleId)
                        .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.ToTable("RolePermissions");
                    j.HasKey(rp => new { rp.RoleId, rp.PermissionId });
                });
    }
}
