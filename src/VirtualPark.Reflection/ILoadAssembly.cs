namespace VirtualPark.Reflection;

public interface ILoadAssembly<out TInterface>
    where TInterface : class
{
    List<string> GetImplementations();
    TInterface GetImplementation(string assemblyName, params object[] args);
}
