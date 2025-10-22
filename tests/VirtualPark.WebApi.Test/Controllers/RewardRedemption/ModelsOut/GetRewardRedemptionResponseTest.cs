using FluentAssertions;
using VirtualPark.WebApi.Controllers.RewardRedemption.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.RewardRedemption.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetRewardRedemptionResponse")]
public sealed class GetRewardRedemptionResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();

        var response = new GetRewardRedemptionResponse(
            id, Guid.NewGuid().ToString());

        response.Id.Should().Be(id);
    }
    #endregion

    #region RewardId
    [TestMethod]
    [TestCategory("Validation")]
    public void RewardId_Getter_ReturnsAssignedValue()
    {
        var rewardId = Guid.NewGuid().ToString();

        var response = new GetRewardRedemptionResponse(
            Guid.NewGuid().ToString(),
            rewardId);

        response.RewardId.Should().Be(rewardId);
    }
    #endregion
}
