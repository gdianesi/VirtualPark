namespace VirtualPark.Reflection;

public sealed class LoadAssembly<ITInterface>(string path)
    where ITInterface : class
{
    private readonly DirectoryInfo directory = new(path);
    private List<Type> implementations = [];
}
