using VirtualPark.BusinessLogic.VisitRegistrations.Entity;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public interface IStrategy
{
    string Key { get; }
    int CalculatePoints(VisitRegistration visitRegistration);
}
