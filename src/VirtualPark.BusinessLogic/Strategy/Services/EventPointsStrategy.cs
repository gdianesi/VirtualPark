using VirtualPark.BusinessLogic.VisitRegistrations.Entity;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public class EventPointsStrategy : IStrategy
{
    public string Key { get; } = "Event";
    public int CalculatePoints(VisitRegistration visitRegistration)
    {
        int points;
        if(visitRegistration.DailyScore == 0)
        {
            points = 20;
        }
        else
        {
            points = visitRegistration.Visitor.Score * 3;
        }

        return points;
    }
}
