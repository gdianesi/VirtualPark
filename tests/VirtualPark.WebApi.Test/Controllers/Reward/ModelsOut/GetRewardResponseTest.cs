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
        var response = new GetRewardResponse(Guid.NewGuid().ToString(), "VIP Ticket", "VIP Entrance", "1500");

        response.Id.Should().Be(id);
    }
    #endregion

    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var response = new GetRewardResponse(Guid.NewGuid().ToString(), "VIP Ticket", "VIP Entrance", "1500");

        response.Name.Should().Be("VIP Ticket");
    }
    #endregion

    #region Description
    [TestMethod]
    [TestCategory("Validation")]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var response = new GetRewardResponse(Guid.NewGuid().ToString(), "VIP Ticket", "VIP Entrance", "1500");

        response.Description.Should().Be("VIP Entrance");
    }
    #endregion

    [TestMethod]
    [TestCategory("Validation")]
    public void PointsRequired_Getter_ReturnsAssignedValue()
    {
        var response = new GetRewardResponse(Guid.NewGuid().ToString(), "VIP Ticket", "VIP Entrance", "1500");

        response.PointsRequired.Should().Be(1500);
    }
}
