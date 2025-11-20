using VirtualPark.BusinessLogic.VisitsScore.Entity;

namespace VirtualPark.BusinessLogic.VisitsScore.Service;

public class VisitScoreService(
    IVisitScoreRepository visitScoreRepository
) : IVisitScoreService
{
    private readonly IVisitScoreRepository _visitScoreRepository = visitScoreRepository;

    public List<VisitScore> GetScoresByVisitorId(Guid visitorId)
    {
        if(visitorId == Guid.Empty)
        {
            throw new ArgumentException("Visitor ID cannot be null.");
        }

        return _visitScoreRepository.ListByVisitorId(visitorId);
    }
}
