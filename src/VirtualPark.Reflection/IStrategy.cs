using VirtualPark.BusinessLogic.VisitRegistrations.Entity;

namespace VirtualPark.Reflection;

public interface IStrategy
{
    string Key { get; }
    int CalculatePoints(VisitRegistration visitRegistration);
}
