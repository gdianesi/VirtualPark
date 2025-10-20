using FluentAssertions;
using VirtualPark.WebApi.Controllers.Reward.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Reward.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetRewardResponse")]
public sealed class GetRewardResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetRewardResponse(id, "VIP Ticket");

        response.Id.Should().Be(id);
    }
    #endregion

    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var response = new GetRewardResponse(Guid.NewGuid().ToString(), "VIP Ticket");

        response.Name.Should().Be("VIP Ticket");
    }
}
