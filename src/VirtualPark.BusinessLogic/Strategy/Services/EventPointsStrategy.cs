using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.ReflectionAbstractions;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public class EventPointsStrategy(IReadOnlyRepository<VisitRegistration> visitRegistrationRepository) : IStrategy
{
    private readonly IReadOnlyRepository<VisitRegistration> _visitRegistrationRepository = visitRegistrationRepository;

    public string Key { get; } = "Event";
    public int CalculatePoints(Guid visitorId)
    {
        var visit = _visitRegistrationRepository.Get(
            v => v.VisitorId == visitorId && v.IsActive,
            include: q => q
                .Include(v => v.Attractions)
                .Include(v => v.Visitor));

        if(visit == null || visit.Attractions == null || visit.Attractions.Count == 0)
        {
            return 0;
        }

        if(visit.DailyScore == 0)
        {
            return 20;
        }

        var visitorScore = visit.Visitor?.Score ?? 0;
        return visitorScore * 3;
    }
}
