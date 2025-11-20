using VirtualPark.BusinessLogic.VisitsScore.Entity;

namespace VirtualPark.BusinessLogic.VisitsScore.Service;

public interface IVisitScoreService
{
    public List<VisitScore> GetScoresByVisitorId(Guid visitorId);
}
