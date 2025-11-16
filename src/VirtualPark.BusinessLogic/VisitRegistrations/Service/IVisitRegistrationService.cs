using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitsScore.Models;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Service;

public interface IVisitRegistrationService
{
    public void RecordVisitScore(RecordVisitScoreArgs args, Guid token);
    public void UpToAttraction(Guid visitId, Guid attractionId);
    public void DownToAttraction(Guid visitId);
    public List<Attraction> GetAttractionsForTicket(Guid visitorId);
    public List<VisitorProfile> GetVisitorsInAttraction(Guid attractionId);
}
