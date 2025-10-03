using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.RolePermissions.Entity;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.UserRoles.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.DataAccess;

public class SqlContext(DbContextOptions<SqlContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<VisitorProfile> VisitorsProfile { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
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
        });

        modelBuilder.Entity<Role>(entity =>
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
        });
    }
}
