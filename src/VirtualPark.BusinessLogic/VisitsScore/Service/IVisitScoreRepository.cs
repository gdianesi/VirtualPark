using VirtualPark.BusinessLogic.VisitsScore.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.VisitsScore.Service;

public interface IVisitScoreRepository : IReadOnlyRepository<VisitScore>
{
    List<VisitScore> ListByVisitorId(Guid visitorId);
}
