using VirtualPark.BusinessLogic.VisitRegistrations.Entity;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public sealed class ComboPointsStrategy : IStrategy
{
    public string Key { get; } = "Combo";

    public int CalculatePoints(VisitRegistration visit)
    {
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
