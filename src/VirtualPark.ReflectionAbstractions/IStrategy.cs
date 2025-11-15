namespace VirtualPark.ReflectionAbstractions;

public interface IStrategy
{
    string Key { get; }
    int CalculatePoints(Guid token);
}
