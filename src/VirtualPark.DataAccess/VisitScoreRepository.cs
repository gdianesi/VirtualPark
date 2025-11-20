using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.VisitsScore.Entity;
using VirtualPark.BusinessLogic.VisitsScore.Service;
using VirtualPark.EntityFrameworkCore;

namespace VirtualPark.DataAccess;

public class VisitScoreRepository(DbContext context)
    : GenericRepository<VisitScore>(context), IVisitScoreRepository
{
    private readonly DbContext _context = context;
    public List<VisitScore> ListByVisitorId(Guid visitorId)
    {
        return [.. _context.Set<VisitScore>()
            .Include(s => s.VisitRegistration)
            .Where(s => s.VisitRegistration.VisitorId == visitorId)
            .OrderByDescending(s => s.OccurredAt)];
    }
}
