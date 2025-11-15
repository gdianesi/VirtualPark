using VirtualPark.BusinessLogic.Sessions.Service;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.ReflectionAbstractions;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public sealed class ComboPointsStrategy(ISessionService sessionService, IReadOnlyRepository<VisitRegistration> visitRegistrationRepository) : IStrategy
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly IReadOnlyRepository<VisitRegistration> _visitRegistrationRepository = visitRegistrationRepository;
    public string Key { get; } = "Combo";

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

        var count = visit.Attractions?.Count ?? 0;
        if(count == 0)
        {
            return 0;
        }

        var points = count * 2;

        if(count > 10)
        {
            points += 10;
        }

        return points;
    }
}
