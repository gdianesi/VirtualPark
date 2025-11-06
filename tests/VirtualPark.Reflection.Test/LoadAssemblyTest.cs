using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        Directory.CreateDirectory(path);

        var loader = new LoadAssembly<IStrategy>(path);

        var result = loader.GetImplementations();

        result.Should().BeEmpty();
    }

    #endregion
}
