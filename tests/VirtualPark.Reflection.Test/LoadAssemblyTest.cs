using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualPark.BusinessLogic.Strategy.Services;
using VirtualPark.Reflection.Test.PluginDoubles;
using VirtualPark.ReflectionAbstractions;

namespace VirtualPark.Reflection.Test;

[TestClass]
[TestCategory("Reflection")]
[TestCategory("LoadAssembly")]
public sealed class LoadAssemblyTest
{
    private string _testPath = null!;

    [TestInitialize]
    public void Setup()
    {
        _testPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            $"PluginsTest_{Guid.NewGuid():N}");

        Directory.CreateDirectory(_testPath);
    }

    [TestCleanup]
    public void Cleanup()
    {
        try
        {
            if(Directory.Exists(_testPath))
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Directory.Delete(_testPath, true);
            }
        }
        catch
        {
        }
    }

    #region GetImplementations

    [TestMethod]
    public void GetImplementations_WhenDirectoryExistsButHasNoDll_ShouldReturnEmpty()
    {
        var loader = new LoadAssembly<IStrategy>(_testPath);

        var result = loader.GetImplementations();

        result.Should().BeEmpty();
    }

    [TestMethod]
    public void GetImplementations_WhenDirectoryHasDllWithoutStrategies_ShouldThrowInvalidOperation()
    {
        var systemDll = typeof(System.Linq.Enumerable).Assembly.Location;
        var destDll = Path.Combine(_testPath, "System.Linq.Copy.dll");
        File.Copy(systemDll, destDll, overwrite: true);

        var loader = new LoadAssembly<IStrategy>(_testPath);

        Action act = () => loader.GetImplementations();

        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("*No strategies found in assembly*System.Linq.Copy.dll*");
    }

    [TestMethod]
    public void GetImplementations_WhenDirectoryHasStrategiesDll_ShouldReturnTypeNames()
    {
        var sourceDll = typeof(AttractionPointsStrategy).Assembly.Location;
        var destDll = Path.Combine(_testPath, "VirtualPark.Strategies.dll");
        File.Copy(sourceDll, destDll, overwrite: true);

        var loader = new LoadAssembly<IStrategy>(_testPath);

        var result = loader.GetImplementations();

        result.Should().NotBeEmpty();
        result.Should().Contain(nameof(AttractionPointsStrategy));
        result.Should().Contain(nameof(ComboPointsStrategy));
        result.Should().Contain(nameof(EventPointsStrategy));
    }

    [TestMethod]
    public void GetImplementations_WhenMixOfStrategyAndNonStrategyDlls_ShouldThrowForNonStrategyDll()
    {
        var strategyDll = typeof(AttractionPointsStrategy).Assembly.Location;
        var strategyDest = Path.Combine(_testPath, "VirtualPark.Strategies.dll");
        File.Copy(strategyDll, strategyDest, overwrite: true);

        var systemDll = typeof(System.Linq.Enumerable).Assembly.Location;
        var systemDest = Path.Combine(_testPath, "System.Linq.Copy.dll");
        File.Copy(systemDll, systemDest, overwrite: true);

        var loader = new LoadAssembly<IStrategy>(_testPath);

        Action act = () => loader.GetImplementations();

        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("*No strategies found in assembly*System.Linq.Copy.dll*");
    }

    [TestMethod]
    public void GetImplementations_ShouldClearPreviousImplementations_WhenCalledMultipleTimes()
    {
        var sourceDll = typeof(AttractionPointsStrategy).Assembly.Location;
        var destDll = Path.Combine(_testPath, "VirtualPark.Strategies.dll");
        File.Copy(sourceDll, destDll, overwrite: true);

        var loader = new LoadAssembly<IStrategy>(_testPath);

        var result1 = loader.GetImplementations();
        var count1 = result1.Count;

        var result2 = loader.GetImplementations();
        var count2 = result2.Count;

        count1.Should().Be(count2);
        result1.Should().BeEquivalentTo(result2);
    }

    [TestMethod]
    public void GetImplementations_ShouldReloadImplementations_WhenNewDllIsAdded()
    {
        var loader = new LoadAssembly<IStrategy>(_testPath);
        var result1 = loader.GetImplementations();

        var sourceDll = typeof(TestStrategy).Assembly.Location;
        var destDll = Path.Combine(_testPath, "TestStrategies.dll");
        File.Copy(sourceDll, destDll, overwrite: true);

        var result2 = loader.GetImplementations();

        result1.Should().BeEmpty();
        result2.Should().NotBeEmpty();
        result2.Should().Contain(nameof(TestStrategy));
    }
    #endregion

    #region GetImplementation

    [TestMethod]
    public void GetImplementation_ShouldThrow_WhenNoImplementationsLoaded()
    {
        var loader = new LoadAssembly<IStrategy>(_testPath);

        Action act = () => loader.GetImplementation("AttractionPointsStrategy");

        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("No implementations loaded.");
    }

    [TestMethod]
    public void GetImplementation_ShouldThrow_WhenKeyNotFound()
    {
        var sourceDll = typeof(AttractionPointsStrategy).Assembly.Location;
        var destDll = Path.Combine(_testPath, "VirtualPark.Strategies.dll");
        File.Copy(sourceDll, destDll, overwrite: true);

        var loader = new LoadAssembly<IStrategy>(_testPath);
        loader.GetImplementations();

        Action act = () => loader.GetImplementation("DoesNotExist");

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Implementation with Key 'DoesNotExist' not found. Available keys:*");
    }

    [TestMethod]
    public void GetImplementation_ShouldCreateInstance_ByKey()
    {
        var sourceDll = typeof(TestStrategy).Assembly.Location;
        var destDll = Path.Combine(_testPath, "TestStrategies.dll");
        File.Copy(sourceDll, destDll, overwrite: true);

        var loader = new LoadAssembly<IStrategy>(_testPath);
        loader.GetImplementations();

        var instance = loader.GetImplementation("Test");

        instance.Should().NotBeNull();
        instance.GetType().Name.Should().Be(nameof(TestStrategy));
        instance.Key.Should().Be("Test");
    }

    [TestMethod]
    public void GetImplementation_ShouldBeCaseInsensitive_WhenMatchingKey()
    {
        var sourceDll = typeof(TestStrategy).Assembly.Location;
        var destDll = Path.Combine(_testPath, "TestStrategies.dll");
        File.Copy(sourceDll, destDll, overwrite: true);

        var loader = new LoadAssembly<IStrategy>(_testPath);
        loader.GetImplementations();

        var lower = loader.GetImplementation("test");
        var upper = loader.GetImplementation("TEST");

        lower.Should().NotBeNull();
        upper.Should().NotBeNull();
        lower.Key.Should().Be("Test");
        upper.Key.Should().Be("Test");
    }

    [TestMethod]
    public void GetImplementation_ShouldThrow_WhenMissingRequiredConstructorArgs()
    {
        var sourceDll = typeof(NeedsArgStrategy).Assembly.Location;
        var destDll = Path.Combine(_testPath, "ArgStrategies.dll");
        File.Copy(sourceDll, destDll, overwrite: true);

        var loader = new LoadAssembly<IStrategy>(_testPath);
        loader.GetImplementations();

        Action act = () => loader.GetImplementation("NeedsArg");

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Implementation with Key 'NeedsArg' not found*Available keys:*");
    }

    [TestMethod]
    public void GetImplementation_ShouldSupportConstructorsWithArguments()
    {
        var sourceDll = typeof(NeedsArgStrategy).Assembly.Location;
        var destDll = Path.Combine(_testPath, "ArgStrategies.dll");
        File.Copy(sourceDll, destDll, overwrite: true);

        var loader = new LoadAssembly<IStrategy>(_testPath);
        loader.GetImplementations();

        var instance = loader.GetImplementation("NeedsArg", "InjectedValue");

        instance.Should().NotBeNull();
        instance.Key.Should().Be("NeedsArg");
        instance.GetType().GetProperty("InitMessage")!.GetValue(instance)
            .Should().Be("InjectedValue");
    }

    #endregion

    #region GetImplementationKeys
    [TestMethod]
    public void GetImplementationKeys_WhenDirectoryExistsButHasNoDll_ShouldReturnEmpty()
    {
        var loader = new LoadAssembly<IStrategy>(_testPath);

        loader.GetImplementations();

        var keys = loader.GetImplementationKeys();

        keys.Should().NotBeNull();
        keys.Should().BeEmpty();
    }

    [TestMethod]
    public void GetImplementationKeys_WhenDirectoryHasTestStrategyDll_ShouldReturnItsKey()
    {
        var sourceDll = typeof(TestStrategy).Assembly.Location;
        var destDll = Path.Combine(_testPath, "TestStrategies.dll");
        File.Copy(sourceDll, destDll, overwrite: true);

        var loader = new LoadAssembly<IStrategy>(_testPath);

        loader.GetImplementations();

        var keys = loader.GetImplementationKeys();

        keys.Should().NotBeNull();
        keys.Should().ContainSingle().Which.Should().Be("Test");
    }

    #endregion
}
