using FluentAssertions;
using Microsoft.Data.Sqlite;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitsScore.Entity;

namespace VirtualPark.DataAccess.Test;

[TestClass]
public class VisitScoreRepositoryTest
{
    private SqliteConnection _connection = null!;
    private SqlContext _context = null!;
    private VisitScoreRepository _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        (_context, _connection) = DbContextBuilder.BuildTestDbContext();
        _repository = new VisitScoreRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        _connection.Close();
        _connection.Dispose();
    }

    [TestMethod]
    public void ListByVisitorId_ReturnsOnlyRequestedVisitor_OrderedDesc_IncludingRegistration()
    {
        var vp1 = new VisitorProfile
        {
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-20)),
            Membership = Membership.Standard
        };
        var vp2 = new VisitorProfile
        {
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)),
            Membership = Membership.Standard
        };

        var tk1 = new Ticket
        {
            Date = DateTime.UtcNow,
            Type = EntranceType.General,
            Visitor = vp1,
            VisitorProfileId = vp1.Id
        };
        var tk2 = new Ticket
        {
            Date = DateTime.UtcNow,
            Type = EntranceType.General,
            Visitor = vp1,
            VisitorProfileId = vp1.Id
        };
        var tkOther = new Ticket
        {
            Date = DateTime.UtcNow,
            Type = EntranceType.General,
            Visitor = vp2,
            VisitorProfileId = vp2.Id
        };

        _context.AddRange(vp1, vp2, tk1, tk2, tkOther);
        _context.SaveChanges();

        var reg1 = new VisitRegistration
        {
            VisitorId = vp1.Id,
            Visitor = vp1,
            TicketId = tk1.Id,
            Ticket = tk1,
            Date = DateTime.UtcNow,
            IsActive = true
        };
        var reg2 = new VisitRegistration
        {
            VisitorId = vp1.Id,
            Visitor = vp1,
            TicketId = tk2.Id,
            Ticket = tk2,
            Date = DateTime.UtcNow,
            IsActive = true
        };
        var regOther = new VisitRegistration
        {
            VisitorId = vp2.Id,
            Visitor = vp2,
            TicketId = tkOther.Id,
            Ticket = tkOther,
            Date = DateTime.UtcNow,
            IsActive = true
        };

        _context.AddRange(reg1, reg2, regOther);
        _context.SaveChanges();

        var older = new VisitScore
        {
            VisitRegistrationId = reg1.Id,
            VisitRegistration = reg1,
            OccurredAt = DateTime.UtcNow.AddHours(-2),
            Points = 10,
            Origin = "A"
        };
        var newer = new VisitScore
        {
            VisitRegistrationId = reg2.Id,
            VisitRegistration = reg2,
            OccurredAt = DateTime.UtcNow,
            Points = 20,
            Origin = "B"
        };
        var other = new VisitScore
        {
            VisitRegistrationId = regOther.Id,
            VisitRegistration = regOther,
            OccurredAt = DateTime.UtcNow.AddHours(-1),
            Points = 5,
            Origin = "C"
        };

        _context.AddRange(older, newer, other);
        _context.SaveChanges();

        var results = _repository.ListByVisitorId(vp1.Id);

        results.Should().NotBeNull();
        results.Should().HaveCount(2, "solo debe traer scores del visitor solicitado");
        results[0].OccurredAt.Should().BeOnOrAfter(results[1].OccurredAt);
        results.Should().OnlyContain(s => s.VisitRegistration != null &&
                                          s.VisitRegistration.VisitorId == vp1.Id);
    }
}
