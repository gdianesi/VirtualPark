using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;

namespace VirtualPark.DataAccess.Test;

[TestClass]
[TestCategory("VisitScoreRepository")]
public class VisitScoreRepositoryTest
{
    private DbContext _ctx = null!;
    private VisitScoreRepository _repo = null!;

    [TestInitialize]
    public void Setup()
    {
        _ctx = SqliteInMemoryDbContext.BuildTestDbContext();
        _ctx.Database.EnsureCreated();
        _repo = new VisitScoreRepository(_ctx);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _ctx.Database.EnsureDeleted();
    }

    private static VisitRegistration VR(Guid visitorId) => new()
    {
        VisitorId = visitorId,
        TicketId = Guid.NewGuid(),
        Date = DateTime.UtcNow,
        IsActive = true
    };
}
