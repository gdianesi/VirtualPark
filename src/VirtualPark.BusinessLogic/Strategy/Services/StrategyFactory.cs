namespace VirtualPark.BusinessLogic.Strategy.Services;

public sealed class StrategyFactory(IEnumerable<IStrategy> strategies) : IStrategyFactory
{
    private readonly Dictionary<string, IStrategy> _strategies = strategies.ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);

    public IStrategy Create(string strategyKey)
    {
        return _strategies.TryGetValue(strategyKey, out var strategy) ? strategy : throw new KeyNotFoundException($"Strategy '{strategyKey}' not found.");
    }
}
