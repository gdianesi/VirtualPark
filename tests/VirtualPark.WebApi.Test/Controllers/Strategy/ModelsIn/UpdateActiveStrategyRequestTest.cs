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
        var request = new UpdateActiveStrategyRequest
        {
            Key = invalidValue
        };

        request.Key.Should().Be(invalidValue);
    }
}
