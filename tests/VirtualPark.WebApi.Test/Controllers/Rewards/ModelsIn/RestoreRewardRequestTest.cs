using FluentAssertions;
using VirtualPark.WebApi.Controllers.Reward.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Rewards.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("RestoreRewardRequest")]
public sealed class RestoreRewardRequestTest
{
    #region QuantityAvailable
    [TestMethod]
    [TestCategory("Validation")]
    public void QuantityAvailable_Getter_ReturnsAssignedValue()
    {
        var request = new RestoreRewardRequest
        {
            QuantityAvailable = "15"
        };

        request.QuantityAvailable.Should().Be("15");
    }
    #endregion
}
