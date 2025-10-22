using FluentAssertions;
using VirtualPark.WebApi.Controllers.RewardRedemption.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.RewardRedemption.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetRewardRedemptionResponse")]
public sealed class GetRewardRedemptionResponseTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();

        var response = new GetRewardRedemptionResponse(
            id);

        response.Id.Should().Be(id);
    }
}
