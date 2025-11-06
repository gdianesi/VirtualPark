using System.Reflection;
using VirtualPark.ReflectionAbstractions;

namespace VirtualPark.Reflection;

public sealed class LoadAssembly<TInterface>(string path) : ILoadAssembly<TInterface>
    where TInterface : class
{
    private readonly DirectoryInfo _directory = new(path);
    private List<Type> _implementations = [];

    public List<string?> GetImplementations()
    {
        var files = _directory.GetFiles("*.dll").ToList();

        files.ForEach(file =>
        {
            Assembly assemblyLoaded = Assembly.LoadFile(file.FullName);
            var loadedTypes = assemblyLoaded
                .GetTypes()
                .Where(t => t.IsClass && typeof(IStrategy).IsAssignableFrom(t))
                .ToList();

            if(loadedTypes.Count == 0)
            {
                throw new InvalidOperationException($"No strategies found in assembly '{file.Name}'.");
            }

            _implementations = _implementations.Union(loadedTypes).ToList();
        });

        return _implementations.ConvertAll(t => t.FullName);
    }

}
