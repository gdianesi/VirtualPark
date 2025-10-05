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

        modelBuilder.Entity<Ranking>(entity =>
        {
            
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
