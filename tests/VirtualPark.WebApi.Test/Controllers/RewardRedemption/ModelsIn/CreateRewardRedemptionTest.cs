using FluentAssertions;
using VirtualPark.WebApi.Controllers.RewardRedemption.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.RewardRedemption.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateRewardRedemptionRequest")]
public class CreateRewardRedemptionRequestTest
{
    #region RewardId
    [TestMethod]
    [TestCategory("Validation")]
    public void RewardId_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var request = new CreateRewardRedemptionRequest { RewardId = id };
        request.RewardId.Should().Be(id);
    }
    #endregion
}
