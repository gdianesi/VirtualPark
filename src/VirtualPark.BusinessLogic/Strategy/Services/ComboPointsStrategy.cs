using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.ReflectionAbstractions;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public sealed class ComboPointsStrategy(IReadOnlyRepository<VisitRegistration> visitRegistrationRepository) : IStrategy
{
    private readonly IReadOnlyRepository<VisitRegistration> _visitRegistrationRepository = visitRegistrationRepository;
    public string Key { get; } = "Combo";

    public int CalculatePoints(Guid visitorId)
    {
        var visit = _visitRegistrationRepository.Get(
            v => v.VisitorId == visitorId && v.IsActive,
            include: q => q.Include(v => v.Attractions));

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
