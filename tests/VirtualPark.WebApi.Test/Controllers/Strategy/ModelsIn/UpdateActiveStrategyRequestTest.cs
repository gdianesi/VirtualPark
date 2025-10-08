using FluentAssertions;
using VirtualPark.WebApi.Controllers.Strategy.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Strategy.ModelsIn;

[TestClass]
[TestCategory("Strategy")]
[TestCategory("UpdateActiveStrategyRequest")]
public class UpdateActiveStrategyRequestTest
{
    [TestMethod]
    public void Key_ShouldSetAndGetValue()
    {
        var request = new UpdateActiveStrategyRequest();

        request.Key = "Event";

        request.Key.Should().Be("Event");
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Key_ShouldAllowNullOrEmpty(string? invalidValue)
    {
        var request = new UpdateActiveStrategyRequest { Key = invalidValue };

        request.Key.Should().Be(invalidValue);
    }

    [TestClass]
    [TestCategory("Strategy")]
    [TestCategory("UpdateActiveStrategyRequest")]
    public class UpdateActiveStrategyRequestTests
    {
        [TestMethod]
        public void ToArgs_ShouldReturnValidArgs_WhenInputIsCorrect()
        {
            var futureDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
            var request = new UpdateActiveStrategyRequest { Key = "Combo" };

            var result = request.ToArgs(futureDate);

            result.Should().NotBeNull();
            result.StrategyKey.Should().Be("Combo");
            result.Date.Should().Be(futureDate);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void ToArgs_ShouldThrow_WhenKeyIsNullOrEmpty(string? invalidKey)
        {
            var date = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
            var request = new UpdateActiveStrategyRequest { Key = invalidKey };

            Action act = () => request.ToArgs(date);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*null or empty*");
        }
    }
}
