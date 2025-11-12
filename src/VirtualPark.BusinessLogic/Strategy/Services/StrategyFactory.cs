using VirtualPark.Reflection;
using VirtualPark.ReflectionAbstractions;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public sealed class StrategyFactory(IEnumerable<IStrategy> strategies, ILoadAssembly<IStrategy> loadAssembly) : IStrategyFactory
{
    private readonly Dictionary<string, IStrategy> _builtInStrategies = strategies.ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, IStrategy> _pluginCache = new(StringComparer.OrdinalIgnoreCase);
    private bool _pluginsDiscovered = false;

    public IStrategy Create(string strategyKey)
    {
        if(string.IsNullOrWhiteSpace(strategyKey))
        {
            throw new ArgumentException("Strategy key cannot be null or empty", nameof(strategyKey));
        }

        if(_builtInStrategies.TryGetValue(strategyKey, out var builtInStrategy))
        {
            return builtInStrategy;
        }

        if(_pluginCache.TryGetValue(strategyKey, out var cachedPlugin))
        {
            return cachedPlugin;
        }

        if(!_pluginsDiscovered)
        {
            DiscoverPlugins();
        }

        try
        {
            var plugin = loadAssembly.GetImplementation(strategyKey);

            if(string.IsNullOrWhiteSpace(plugin.Key))
            {
                throw new InvalidOperationException($"Plugin '{strategyKey}' has no valid Key defined.");
            }

            _pluginCache[plugin.Key] = plugin;

            return plugin;
        }
        catch(Exception ex)
        {
            throw new KeyNotFoundException(
                $"Strategy '{strategyKey}' not found in built-in strategies or plugins. Details: {ex.Message}");
        }
    }

    private void DiscoverPlugins()
    {
        try
        {
            loadAssembly.GetImplementations();
            _pluginsDiscovered = true;
        }
        catch
        {
            _pluginsDiscovered = true;
        }
    }
}
