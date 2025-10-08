using FluentAssertions;
using VirtualPark.WebApi.Controllers.Strategy.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Strategy.ModelsIn;

[TestClass]
[TestCategory("Strategy")]
[TestCategory("CreateActiveStrategyRequest")]
public class CreateActiveStrategyRequestTests
{
    [TestMethod]
    public void ToArgs_ShouldReturnValidArgs_WhenInputIsCorrect()
    {
        var futureDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1).ToString("yyyy-MM-dd");
        var request = new CreateActiveStrategyRequest
        {
            StrategyKey = "Event",
            Date = futureDate
        };

        var result = request.ToArgs();

        result.Should().NotBeNull();
        result.StrategyKey.Should().Be("Event");
        result.Date.Should().Be(DateOnly.Parse(futureDate));
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void ToArgs_ShouldThrow_WhenStrategyKeyIsNullOrEmpty(string? invalidKey)
    {
        var futureDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1).ToString("yyyy-MM-dd");
        var request = new CreateActiveStrategyRequest
        {
            StrategyKey = invalidKey,
            Date = futureDate
        };

        Action act = () => request.ToArgs();

        act.Should().Throw<ArgumentException>()
            .WithMessage("*null or empty*");
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void ToArgs_ShouldThrow_WhenDateIsNullOrEmpty(string? invalidDate)
    {
        var request = new CreateActiveStrategyRequest
        {
            StrategyKey = "Combo",
            Date = invalidDate
        };

        Action act = () => request.ToArgs();

        act.Should().Throw<ArgumentException>()
            .WithMessage("*null or empty*");
    }

    [TestMethod]
    public void ToArgs_ShouldThrow_WhenDateIsInvalidFormat()
    {
        var request = new CreateActiveStrategyRequest
        {
            StrategyKey = "Attraction",
            Date = "07-10-2028"
        };

        Action act = () => request.ToArgs();

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Invalid date*");
    }

    [TestMethod]
    public void ToArgs_ShouldThrow_WhenDateIsInThePast()
    {
        var pastDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-1).ToString("yyyy-MM-dd");
        var request = new CreateActiveStrategyRequest
        {
            StrategyKey = "Event",
            Date = pastDate
        };

        Action act = () => request.ToArgs();

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Date cannot be in the past*");
    }
}
