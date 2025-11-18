using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Attractions;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.ReflectionAbstractions;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public class AttractionPointsStrategy(IReadOnlyRepository<VisitRegistration> visitRegistrationRepository) : IStrategy
{
    private readonly IReadOnlyRepository<VisitRegistration> _visitRegistrationRepository = visitRegistrationRepository;

    public string Key { get; } = "Attraction";

    public int CalculatePoints(Guid visitorId)
    {
        var visit = _visitRegistrationRepository.Get(
            v => v.VisitorId == visitorId && v.IsActive,
            include: q => q
                .Include(v => v.Attractions));

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
