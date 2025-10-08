using FluentAssertions;
using VirtualPark.BusinessLogic.Strategy.Models;

namespace VirtualPark.BusinessLogic.Test.Strategy.Models;

[TestClass]
public class ActiveStrategyArgsTests
{
    #region Valid Cases

    [TestMethod]
    public void Constructor_ShouldAssignValues_WhenParametersAreValid()
    {
        var args = new ActiveStrategyArgs("Combo", "2029-10-07");

        args.StrategyKey.Should().Be("Combo");
        args.Date.Should().Be(DateOnly.Parse("2029-10-07"));
    }

    #endregion

    #region Invalid StrategyKey

    [TestMethod]
    public void Constructor_ShouldThrowArgumentException_WhenStrategyKeyIsNull()
    {
        var act = () => new ActiveStrategyArgs(null!, "2025-10-07");

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_ShouldThrowArgumentException_WhenStrategyKeyIsEmpty()
    {
        var act = () => new ActiveStrategyArgs(string.Empty, "2025-10-07");

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Constructor_ShouldThrowArgumentException_WhenStrategyKeyIsWhitespace()
    {
        var act = () => new ActiveStrategyArgs("   ", "2025-10-07");

        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #region Invalid Date

    [TestMethod]
    public void Constructor_ShouldThrowArgumentException_WhenDateHasWrongFormat()
    {
        var act = () => new ActiveStrategyArgs("Combo", "07/10/2025");
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Invalid date format*");
    }

    #endregion
}
