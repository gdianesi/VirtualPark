using VirtualPark.BusinessLogic.Attractions;

using VirtualPark.BusinessLogic.Sessions.Service;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.ReflectionAbstractions;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public class AttractionPointsStrategy(ISessionService sessionService, IReadOnlyRepository<VisitRegistration> visitRegistrationRepository) : IStrategy
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly IReadOnlyRepository<VisitRegistration> _visitRegistrationRepository = visitRegistrationRepository;

    public string Key { get; } = "Attraction";

    public int CalculatePoints(Guid token)
    {
        var user = _sessionService.GetUserLogged(token);

        if(user == null)
        {
            throw new ApplicationException("No user logged");
        }

        var visit = _visitRegistrationRepository.Get(v => v.Visitor.Id == user.VisitorProfileId && v.IsActive);

        if(visit == null || visit.Attractions == null || visit.Attractions.Count == 0)
        {
            return 0;
        }

        var totalPoints = visit.Attractions.Sum(a => a.Type switch
        {
            AttractionType.RollerCoaster => 50,
            AttractionType.Show => 30,
            AttractionType.Simulator => 20,
            _ => 10
        });

        return totalPoints;
    }
}
