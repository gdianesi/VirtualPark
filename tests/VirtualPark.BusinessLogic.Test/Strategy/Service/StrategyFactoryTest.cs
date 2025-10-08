using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Strategy.Services;

namespace VirtualPark.BusinessLogic.Test.Strategy.Service;

[TestClass]
public class StrategyFactoryTests
{
    [TestMethod]
    public void Create_ShouldReturnStrategy_WhenKeyExists_ExactMatch()
    {
        var attraction = new Mock<IStrategy>(MockBehavior.Strict);
        attraction.SetupGet(s => s.Key).Returns("Attraction");

        var combo = new Mock<IStrategy>(MockBehavior.Strict);
        combo.SetupGet(s => s.Key).Returns("Combo");

        var evt = new Mock<IStrategy>(MockBehavior.Strict);
        evt.SetupGet(s => s.Key).Returns("Event");

        var factory = new StrategyFactory([attraction.Object, combo.Object, evt.Object]);

        var result = factory.Create("Combo");

        result.Should().BeSameAs(combo.Object);
    }

    [TestMethod]
    public void Create_ShouldBeCaseInsensitive()
    {
        var attraction = new Mock<IStrategy>(MockBehavior.Strict);
        attraction.SetupGet(s => s.Key).Returns("Attraction");

        var factory = new StrategyFactory([attraction.Object]);

        var lower = factory.Create("attraction");
        var upper = factory.Create("ATTRACTION");

        lower.Should().BeSameAs(attraction.Object);
        upper.Should().BeSameAs(attraction.Object);
    }

    [TestMethod]
    public void Create_ShouldThrowKeyNotFound_WhenKeyDoesNotExist()
    {
        var attraction = new Mock<IStrategy>(MockBehavior.Strict);
        attraction.SetupGet(s => s.Key).Returns("Attraction");

        var combo = new Mock<IStrategy>(MockBehavior.Strict);
        combo.SetupGet(s => s.Key).Returns("Combo");

        var evt = new Mock<IStrategy>(MockBehavior.Strict);
        evt.SetupGet(s => s.Key).Returns("Event");

        var factory = new StrategyFactory([attraction.Object, combo.Object, evt.Object]);

        var act = () => factory.Create("Unknown");

        act.Should()
            .Throw<KeyNotFoundException>()
            .WithMessage("Strategy 'Unknown' not found.");
    }

    [TestMethod]
    public void Ctor_ShouldThrowArgumentException_WhenDuplicateKeys()
    {
        var s1 = new Mock<IStrategy>(MockBehavior.Strict);
        s1.SetupGet(s => s.Key).Returns("Attraction");

        var s2 = new Mock<IStrategy>(MockBehavior.Strict);
        s2.SetupGet(s => s.Key).Returns("Attraction");

        var act = () => new StrategyFactory([s1.Object, s2.Object]);

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Ctor_ShouldThrowArgumentException_WhenDuplicateKeysDifferOnlyByCase()
    {
        var s1 = new Mock<IStrategy>(MockBehavior.Strict);
        s1.SetupGet(s => s.Key).Returns("Combo");

        var s2 = new Mock<IStrategy>(MockBehavior.Strict);
        s2.SetupGet(s => s.Key).Returns("combo");

        var act = () => new StrategyFactory([s1.Object, s2.Object]);

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Ctor_ShouldThrowArgumentNullException_WhenEnumerableIsNull()
    {
        IEnumerable<IStrategy> nullEnumerable = null!;

        var act = () => new StrategyFactory(nullEnumerable);

        act.Should().Throw<ArgumentNullException>();
    }
}
