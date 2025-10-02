using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Roles.Entity;
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
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Password).IsRequired();

            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasOne(e => e.VisitorProfile)
                .WithOne()
                .HasForeignKey<User>(e => e.VisitorProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Roles)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "UserRoles",
                    j => j.HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.ToTable("UserRoles");
                        j.HasKey("UserId", "RoleId");
                        j.HasIndex("RoleId");
                    });
        });
    }
}
