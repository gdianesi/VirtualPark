using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPark.BusinessLogic.Permissions.Entity;

namespace VirtualPark.DataAccess.Configurations;

[ExcludeFromCodeCoverage]
public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> entity)
    {
        entity.ToTable("Permissions");

        entity.HasKey(p => p.Id);

        entity.Property(p => p.Id).ValueGeneratedNever();

        entity.Property(p => p.Description).IsRequired();

        entity.Property(p => p.Key).IsRequired();

        entity.HasIndex(p => p.Key).IsUnique();
    }
}
