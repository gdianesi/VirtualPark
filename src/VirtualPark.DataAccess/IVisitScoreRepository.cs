using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitsScore.Entity;
using VirtualPark.Repository;

namespace VirtualPark.DataAccess;

public interface IVisitScoreRepository : IReadOnlyRepository<VisitScore>
{
    List<VisitScore> ListByVisitorId(Guid visitorId);
}
