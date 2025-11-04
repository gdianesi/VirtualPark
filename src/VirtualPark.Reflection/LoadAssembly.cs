namespace VirtualPark.Reflection;

public sealed class LoadAssembly<ITInterface>(string path) : ILoadAssembly<ITInterface>
    where ITInterface : class
{
    private readonly DirectoryInfo directory = new(path);
    private List<Type> implementations = [];
    public List<string> GetImplementations()
    {
        throw new NotImplementedException();
    }

    public ITInterface GetImplementation(string assemblyName, params object[] args)
    {
        throw new NotImplementedException();
    }
}
