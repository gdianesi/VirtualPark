using VirtualPark.BusinessLogic.Attractions;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public class AttractionPointsStrategy : IStrategy
{
    public string Key { get; } = "Attraction";

    public int CalculatePoints(VisitRegistration visit)
    {
        if(visit.Attractions.Count == 0)
        {
            return 0;
        }

        var totalPoints = 0;

        foreach(var attraction in visit.Attractions)
        {
            totalPoints += attraction.Type switch
            {
                AttractionType.RollerCoaster => 50,
                AttractionType.Show => 30,
                AttractionType.Simulator => 20,
                _ => 10
            };
        }

        return totalPoints;
    }
}
