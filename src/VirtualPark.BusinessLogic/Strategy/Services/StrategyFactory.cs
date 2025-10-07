using System.Collections.Generic;
using System.Linq;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public sealed class StrategyFactory : IStrategyFactory
{
    private readonly Dictionary<string, IStrategy> _strategies;

    public StrategyFactory(IEnumerable<IStrategy> strategies)
    {
        _strategies = strategies.ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);
    }

    public IStrategy Create(string strategyKey)
    {
        return _strategies.TryGetValue(strategyKey, out var strategy) ? strategy : throw new KeyNotFoundException($"Estrategia '{strategyKey}' no encontrada.");
    }
}
