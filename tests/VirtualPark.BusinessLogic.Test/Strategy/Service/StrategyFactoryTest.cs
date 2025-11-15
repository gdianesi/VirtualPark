using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Strategy.Services;
using VirtualPark.Reflection;
using VirtualPark.ReflectionAbstractions;

namespace VirtualPark.BusinessLogic.Test.Strategy.Service;

[TestClass]
public class StrategyFactoryTests
{
    [TestMethod]
    public void Create_ShouldReturnStrategy_WhenKeyExists_ExactMatch()
    {
        var loader = new Mock<ILoadAssembly<IStrategy>>(MockBehavior.Strict);

        loader.Setup(l => l.GetImplementations()).Returns([]);

        loader.Setup(l => l.GetImplementation(It.IsAny<string>(), It.IsAny<object[]>()))
            .Throws(new InvalidOperationException("not found"));

        var attraction = new Mock<IStrategy>(MockBehavior.Strict);
        attraction.SetupGet(s => s.Key).Returns("Attraction");

        var combo = new Mock<IStrategy>(MockBehavior.Strict);
        combo.SetupGet(s => s.Key).Returns("Combo");

        var evt = new Mock<IStrategy>(MockBehavior.Strict);
        evt.SetupGet(s => s.Key).Returns("Event");

        var factory = new StrategyFactory(
            [attraction.Object, combo.Object, evt.Object],
            loader.Object);

        var result = factory.Create("Combo");

        result.Should().BeSameAs(combo.Object);
    }

    [TestMethod]
    public void Create_ShouldBeCaseInsensitive()
    {
        var loader = new Mock<ILoadAssembly<IStrategy>>(MockBehavior.Strict);
        loader.Setup(l => l.GetImplementations()).Returns([]);
        loader.Setup(l => l.GetImplementation(It.IsAny<string>(), It.IsAny<object[]>()))
            .Throws(new InvalidOperationException("not found"));

        var attraction = new Mock<IStrategy>(MockBehavior.Strict);
        attraction.SetupGet(s => s.Key).Returns("Attraction");

        var factory = new StrategyFactory(
            [attraction.Object],
            loader.Object);

        var lower = factory.Create("attraction");
        var upper = factory.Create("ATTRACTION");

        lower.Should().BeSameAs(attraction.Object);
        upper.Should().BeSameAs(attraction.Object);
    }

    [TestMethod]
    public void Create_ShouldThrowKeyNotFound_WhenKeyDoesNotExist()
    {
        var loader = new Mock<ILoadAssembly<IStrategy>>(MockBehavior.Strict);
        loader.Setup(l => l.GetImplementations()).Returns([]);
        loader.Setup(l => l.GetImplementation(It.IsAny<string>(), It.IsAny<object[]>()))
            .Throws(new InvalidOperationException("not found"));

        var attraction = new Mock<IStrategy>(MockBehavior.Strict);
        attraction.SetupGet(s => s.Key).Returns("Attraction");

        var combo = new Mock<IStrategy>(MockBehavior.Strict);
        combo.SetupGet(s => s.Key).Returns("Combo");

        var evt = new Mock<IStrategy>(MockBehavior.Strict);
        evt.SetupGet(s => s.Key).Returns("Event");

        var factory = new StrategyFactory(
            [attraction.Object, combo.Object, evt.Object],
            loader.Object);

        var act = () => factory.Create("Unknown");

        act.Should()
            .Throw<KeyNotFoundException>()
            .WithMessage("Strategy 'Unknown' not found in built-in strategies or plugins*");
    }

    [TestMethod]
    public void Ctor_ShouldThrowArgumentException_WhenDuplicateKeys()
    {
        var s1 = new Mock<IStrategy>(MockBehavior.Strict);
        s1.SetupGet(s => s.Key).Returns("Attraction");

        var s2 = new Mock<IStrategy>(MockBehavior.Strict);
        s2.SetupGet(s => s.Key).Returns("Attraction");

        var loader = new Mock<ILoadAssembly<IStrategy>>(MockBehavior.Strict);
        loader.Setup(l => l.GetImplementations()).Returns([]);
        loader.Setup(l => l.GetImplementation(It.IsAny<string>(), It.IsAny<object[]>()))
            .Throws(new InvalidOperationException("not found"));

        var act = () => new StrategyFactory(
            [s1.Object, s2.Object],
            loader.Object);

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Ctor_ShouldThrowArgumentException_WhenDuplicateKeysDifferOnlyByCase()
    {
        var s1 = new Mock<IStrategy>(MockBehavior.Strict);
        s1.SetupGet(s => s.Key).Returns("Combo");

        var s2 = new Mock<IStrategy>(MockBehavior.Strict);
        s2.SetupGet(s => s.Key).Returns("combo");

        var loader = new Mock<ILoadAssembly<IStrategy>>(MockBehavior.Strict);
        loader.Setup(l => l.GetImplementations()).Returns([]);
        loader.Setup(l => l.GetImplementation(It.IsAny<string>(), It.IsAny<object[]>()))
            .Throws(new InvalidOperationException("not found"));

        var act = () => new StrategyFactory(
            [s1.Object, s2.Object],
            loader.Object);

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Ctor_ShouldThrowArgumentNullException_WhenEnumerableIsNull()
    {
        IEnumerable<IStrategy> nullEnumerable = null!;

        var loader = new Mock<ILoadAssembly<IStrategy>>(MockBehavior.Strict);
        loader.Setup(l => l.GetImplementations()).Returns([]);
        loader.Setup(l => l.GetImplementation(It.IsAny<string>(), It.IsAny<object[]>()))
            .Throws(new InvalidOperationException("not found"));

        var act = () => new StrategyFactory(nullEnumerable, loader.Object);

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void DiscoverPlugins_ShouldSetPluginsDiscovered_WhenLoadSucceeds()
    {
        var loader = new Mock<ILoadAssembly<IStrategy>>(MockBehavior.Strict);
        loader.Setup(l => l.GetImplementations()).Returns(["Plugins.PuntuacionPorHora"]);

        var factory = new StrategyFactory(Array.Empty<IStrategy>(), loader.Object);

        var method = typeof(StrategyFactory).GetMethod("DiscoverPlugins", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
        method.Invoke(factory, null);

        var field = typeof(StrategyFactory).GetField("_pluginsDiscovered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
        var value = (bool)field.GetValue(factory)!;

        value.Should().BeTrue();
        loader.Verify(l => l.GetImplementations(), Times.Once);
    }

    [TestMethod]
    public void DiscoverPlugins_ShouldSetPluginsDiscovered_WhenLoadThrows()
    {
        var loader = new Mock<ILoadAssembly<IStrategy>>(MockBehavior.Strict);
        loader.Setup(l => l.GetImplementations()).Throws(new InvalidOperationException("Bad DLL"));

        var factory = new StrategyFactory(Array.Empty<IStrategy>(), loader.Object);

        var method = typeof(StrategyFactory).GetMethod("DiscoverPlugins", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
        method.Invoke(factory, null);

        var field = typeof(StrategyFactory).GetField("_pluginsDiscovered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
        var value = (bool)field.GetValue(factory)!;

        value.Should().BeTrue();
        loader.Verify(l => l.GetImplementations(), Times.Once);
    }
}
