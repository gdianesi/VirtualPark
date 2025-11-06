using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualPark.BusinessLogic.Strategy.Services;
using VirtualPark.ReflectionAbstractions;

namespace VirtualPark.Reflection.Test;

[TestClass]
[TestCategory("Reflection")]
[TestCategory("LoadAssembly")]
public sealed class LoadAssemblyTest
{
    #region GetImplementations

    [TestMethod]
    public void GetImplementations_WhenDirectoryExistsButHasNoDll_ShouldReturnEmpty()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PluingsTestDllTest");
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        Directory.CreateDirectory(path);

        var loader = new LoadAssembly<IStrategy>(path);

        var result = loader.GetImplementations();

        result.Should().BeEmpty();
    }

    [TestMethod]
    public void GetImplementations_WhenDirectoryHasDllWithoutStrategies_ShouldThrowInvalidOperation()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PluingsTestDllTest_NoStrategies");
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        Directory.CreateDirectory(path);

        var sourceDll = typeof(LoadAssemblyTest).Assembly.Location;
        var destDll = Path.Combine(path, Path.GetFileName(sourceDll));
        File.Copy(sourceDll, destDll, true);

        var loader = new LoadAssembly<IStrategy>(path);

        Action act = () => loader.GetImplementations();

        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage($"No strategies found in assembly '{Path.GetFileName(destDll)}'.");
    }

    #endregion
}
