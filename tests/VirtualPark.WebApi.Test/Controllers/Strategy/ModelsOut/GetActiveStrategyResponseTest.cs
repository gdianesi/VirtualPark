using FluentAssertions;
using VirtualPark.BusinessLogic.Strategy.Models;
using VirtualPark.WebApi.Controllers.Strategy.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Strategy.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetActiveStrategyResponse")]
public class GetActiveStrategyResponseTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WithValidStrategy_ShouldAssignProperties()
    {
        var args = new ActiveStrategyArgs(
            strategyKey: "Event",
            date: "2029-10-08");

        var response = new GetActiveStrategyResponse(args);

        response.Key.Should().Be("Event");
        response.Date.Should().Be("2029-10-08");
    }
}
