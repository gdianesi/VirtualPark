using VirtualPark.BusinessLogic.VisitsScore.Models;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Service;

public interface IVisitRegistrationService
{
    public void RecordVisitScore(RecordVisitScoreArgs args, Guid token);
}
