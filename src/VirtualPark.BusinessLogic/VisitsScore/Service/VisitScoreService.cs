using VirtualPark.BusinessLogic.VisitsScore.Entity;
using VirtualPark.DataAccess;

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
            throw new ArgumentException("Visitor ID no puede ser vacío.");
        }

        return _visitScoreRepository.ListByVisitorId(visitorId);
    }
}
