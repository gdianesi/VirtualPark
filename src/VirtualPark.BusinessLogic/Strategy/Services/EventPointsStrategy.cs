using VirtualPark.BusinessLogic.VisitRegistrations.Entity;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public class EventPointsStrategy : IStrategy
{
    public string Key { get; } = "Event";
    public int CalculatePoints(VisitRegistration visitRegistration)
    {
        var points = 0;

        if(visitRegistration.Ticket.Event != null)
        {
            points = visitRegistration.Visitor.Score * 3;
        }

        return points;
    }
}
