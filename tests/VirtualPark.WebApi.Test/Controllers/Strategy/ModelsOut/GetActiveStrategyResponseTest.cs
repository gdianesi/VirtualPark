using FluentAssertions;
using VirtualPark.WebApi.Controllers.Strategy.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Strategy.ModelsOut;

[TestClass]
public class GetActiveStrategyResponseTest
{
    [TestMethod]
    public void Constructor_ShouldAssignValues()
    {
        _ = Guid.NewGuid().ToString();
        var key = "Event";
        var date = "2029-10-08";

        var response = new GetActiveStrategyResponse(key, date);

        response.Key.Should().Be(key);
        response.Date.Should().Be(date);
    }

    [TestMethod]
    public void Constructor_ShouldAllowNullValues()
    {
        var response = new GetActiveStrategyResponse(null!, null!);

        response.Key.Should().BeNull();
        response.Date.Should().BeNull();
    }
}
