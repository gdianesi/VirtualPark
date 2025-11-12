using System.Reflection;
using VirtualPark.ReflectionAbstractions;

namespace VirtualPark.Reflection;

public sealed class LoadAssembly<TInterface>(string path) : ILoadAssembly<TInterface>
    where TInterface : class
{
    private readonly DirectoryInfo _directory = new(path);
    private readonly List<Type> _implementations = [];

    public List<string> GetImplementations()
    {
        _implementations.Clear();
        var files = _directory.GetFiles("*.dll");

        foreach(var file in files)
        {
            var asm = Assembly.LoadFile(file.FullName);

            var types = asm.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(TInterface).IsAssignableFrom(t))
                .ToList();

            if(types.Count == 0)
            {
                throw new InvalidOperationException($"No strategies found in assembly '{file.Name}'.");
            }

            _implementations.AddRange(types);
        }

        return _implementations.Select(t => t.Name).ToList();
    }

    public List<string> GetImplementationKeys()
    {
        if(_implementations.Count == 0)
        {
            GetImplementations();
        }

        var keys = new List<string>();
        foreach(var t in _implementations)
        {
            try
            {
                if(Activator.CreateInstance(t) is IStrategy s && !string.IsNullOrWhiteSpace(s.Key))
                {
                    keys.Add(s.Key);
                }
            }
            catch
            {
                // ignored
            }
        }

        return keys.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    public TInterface GetImplementation(string key, params object[] args)
    {
        if(_implementations == null || _implementations.Count == 0)
        {
            throw new InvalidOperationException("No implementations loaded.");
        }

        foreach(var type in _implementations)
        {
            try
            {
                if(Activator.CreateInstance(type, args) is IStrategy strategy &&
                    !string.IsNullOrWhiteSpace(strategy.Key) &&
                    string.Equals(strategy.Key, key, StringComparison.OrdinalIgnoreCase))
                {
                    return (TInterface)(object)strategy;
                }
            }
            catch
            {
                continue;
            }
        }

        var availableKeys = GetImplementationKeys();
        throw new InvalidOperationException(
            $"Implementation with Key '{key}' not found. Available keys: {string.Join(", ", availableKeys)}");
    }
}
