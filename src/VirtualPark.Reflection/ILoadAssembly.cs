namespace VirtualPark.Reflection;

public interface ILoadAssembly<out TInterface>
    where TInterface : class
{
    List<string> GetImplementations();
    List<string> GetImplementationKeys();
    TInterface GetImplementation(string assemblyName, params object[] args);
}
