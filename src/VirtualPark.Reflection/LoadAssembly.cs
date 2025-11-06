using System.Reflection;
using VirtualPark.ReflectionAbstractions;

namespace VirtualPark.Reflection;

public sealed class LoadAssembly<TInterface>(string path) : ILoadAssembly<TInterface>
    where TInterface : class
{
    private readonly DirectoryInfo _directory = new(path);
    private List<Type> _implementations = [];

    
}
