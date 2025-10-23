using FluentAssertions;
using VirtualPark.WebApi.Controllers.Reward.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Rewards.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("CreateRewardResponse")]
public class CreateRewardResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();

        var response = new CreateRewardResponse(id);

        response.Id.Should().Be(id);
    }
    #endregion
}
