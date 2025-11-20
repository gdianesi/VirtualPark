using VirtualPark.ReflectionAbstractions;

namespace VirtualPark.Reflection.Test.PluginDoubles;

public sealed class TestStrategy : IStrategy
{
    public string Key { get; } = "Test";
    public int CalculatePoints(Guid visitorId) => 123;
}
