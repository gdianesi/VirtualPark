using FluentAssertions;
using VirtualPark.WebApi.Controllers.RewardRedemption.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.RewardRedemption.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("CreateRewardRedemptionResponse")]
public class CreateRewardRedemptionResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();

        var response = new CreateRewardRedemptionResponse(id);

        response.Id.Should().Be(id);
    }
    #endregion
}
