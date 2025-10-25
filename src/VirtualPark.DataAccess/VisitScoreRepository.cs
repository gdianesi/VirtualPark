using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.VisitsScore.Entity;

namespace VirtualPark.DataAccess;

public class VisitScoreRepository(DbContext context)
    : GenericRepository<VisitScore>(context), IVisitScoreRepository
{
    public List<VisitScore> ListByVisitorId(Guid visitorId)
    {
        return Context.Set<VisitScore>()
            .Include(s => s.VisitRegistration)
            .Where(s => s.VisitRegistration.VisitorId == visitorId)
            .OrderByDescending(s => s.OccurredAt)
            .ToList();
    }
}
