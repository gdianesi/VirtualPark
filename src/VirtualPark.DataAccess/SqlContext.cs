using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Rankings.Entity;
using VirtualPark.BusinessLogic.RewardRedemptions.Entity;
using VirtualPark.BusinessLogic.Rewards.Entity;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Sessions.Entity;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.DataAccess.Seed;

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
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Reward> Rewards { get; set; }
    public DbSet<RewardRedemption> RewardRedemptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlContext).Assembly);

        SeedData.Seed(modelBuilder);
    }
}
