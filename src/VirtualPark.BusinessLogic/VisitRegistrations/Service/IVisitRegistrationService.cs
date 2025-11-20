using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Models;
using VirtualPark.BusinessLogic.VisitsScore.Models;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Service;

public interface IVisitRegistrationService
{
    public void RecordVisitScore(RecordVisitScoreArgs args);
    public void UpToAttraction(Guid visitId, Guid attractionId);
    public void DownToAttraction(Guid visitId);
    public List<Attraction> GetAttractionsForTicket(Guid visitorId);
    public List<VisitorInAttraction> GetVisitorsInAttraction(Guid attractionId);
    public VisitRegistration GetTodayVisit(Guid visitorId);
}
