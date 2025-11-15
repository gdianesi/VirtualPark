using VirtualPark.ReflectionAbstractions;

namespace VirtualPark.Reflection.Test.PluginDoubles;

public sealed class NeedsArgStrategy(string message) : IStrategy
{
    public string InitMessage { get; } = message;

    public string Key => "NeedsArg";

    public int CalculatePoints(Guid token)
    {
        return InitMessage?.Length ?? 0;
    }
}
