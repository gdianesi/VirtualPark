using VirtualPark.BusinessLogic.Sessions.Service;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.ReflectionAbstractions;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public class EventPointsStrategy(ISessionService sessionService, IReadOnlyRepository<VisitRegistration> visitRegistrationRepository) : IStrategy
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly IReadOnlyRepository<VisitRegistration> _visitRegistrationRepository = visitRegistrationRepository;

    private const int FirstVisitPoints = 20;
    private const int ScoreMultiplier = 3;

    public string Key { get; } = "Event";

    public int CalculatePoints(Guid token)
    {
        var user = _sessionService.GetUserLogged(token)
                   ?? throw new ApplicationException("No user logged");

        var visit = _visitRegistrationRepository
            .Get(v => v.Visitor.Id == user.VisitorProfileId && v.IsActive);

        if (visit == null || visit.Attractions == null || visit.Attractions.Count == 0)
        {
            return 0;
        }

        if (visit.DailyScore == 0)
        {
            return FirstVisitPoints;
        }

        return visit.Visitor.Score * ScoreMultiplier;
    }
}
