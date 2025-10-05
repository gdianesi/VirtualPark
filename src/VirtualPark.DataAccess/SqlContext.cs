using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.AttractionsEvents.Entity;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Rankings.Entity;
using VirtualPark.BusinessLogic.RolePermissions.Entity;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.BusinessLogic.UserRoles.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitsRegistrationsAttractions.Entity;

namespace VirtualPark.DataAccess;

public class SqlContext(DbContextOptions<SqlContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<VisitorProfile> VisitorsProfile { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Attraction> Attractions { get; set; }
    public DbSet<TypeIncidence> TypeIncidences { get; set; }
    public DbSet<Incidence> Incidences { get; set; }
    public DbSet<Ranking> Rankings { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<VisitRegistration> VisitRegistrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Event>(entity =>
        {
            
        });

        modelBuilder.Entity<Attraction>(entity =>
        {
            entity.ToTable("Attractions");

            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).ValueGeneratedNever();

            entity.Property(a => a.Type).IsRequired();
            entity.Property(a => a.Name).IsRequired();
            entity.Property(a => a.MiniumAge).IsRequired();
            entity.Property(a => a.Capacity).IsRequired();
            entity.Property(a => a.Description).IsRequired();
            entity.Property(a => a.CurrentVisitors).HasDefaultValue(0);
            entity.Property(a => a.Available).HasDefaultValue(true);
        });

        modelBuilder.Entity<TypeIncidence>(entity =>
        {
            entity.ToTable("TypeIncidences");

            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).ValueGeneratedNever();

            entity.Property(t => t.Type).IsRequired();

            entity.HasIndex(t => t.Type).IsUnique();
        });

        modelBuilder.Entity<Incidence>(entity =>
        {
            entity.ToTable("Incidences");

            entity.HasKey(i => i.Id);
            entity.Property(i => i.Id).ValueGeneratedNever();

            entity.Property(i => i.Description).IsRequired();
            entity.Property(i => i.Start).HasColumnType("datetime2");
            entity.Property(i => i.End).HasColumnType("datetime2");
            entity.Property(i => i.Active).HasDefaultValue(true);

            entity
                .HasOne(i => i.Type)
                .WithMany()
                .HasForeignKey(i => i.TypeIncidenceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(i => i.Attraction)
                .WithMany()
                .HasForeignKey(i => i.AttractionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(i => i.TypeIncidenceId);
            entity.HasIndex(i => i.AttractionId);
        });

        modelBuilder.Entity<Ranking>(entity =>
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
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Tickets");

            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).ValueGeneratedNever();

            entity.Property(t => t.Date).HasColumnType("datetime2");
            entity.Property(t => t.Type).IsRequired();
            entity.Property(t => t.QrId).IsRequired();

            entity
                .HasOne(t => t.Event)
                .WithMany()
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(t => t.Visitor)
                .WithMany()
                .HasForeignKey(t => t.VisitorProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(t => t.EventId);
            entity.HasIndex(t => t.VisitorProfileId);
            entity.HasIndex(t => t.QrId).IsUnique();
        });
    }
}
