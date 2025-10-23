using FluentAssertions;
using VirtualPark.WebApi.Controllers.Reward.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Rewards.ModelsOut;

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
        var response = new GetRewardResponse(id, "VIP Ticket", "VIP Entrance", "1500", "20", "VIP");

        response.Id.Should().Be(id);
    }
    #endregion

    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var response = new GetRewardResponse(Guid.NewGuid().ToString(), "VIP Ticket", "VIP Entrance", "1500", "20", "VIP");

        response.Name.Should().Be("VIP Ticket");
    }
    #endregion

    #region Description
    [TestMethod]
    [TestCategory("Validation")]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var response = new GetRewardResponse(Guid.NewGuid().ToString(), "VIP Ticket", "VIP Entrance", "1500", "20", "VIP");

        response.Description.Should().Be("VIP Entrance");
    }
    #endregion

    #region Cost
    [TestMethod]
    [TestCategory("Validation")]
    public void PointsRequired_Getter_ReturnsAssignedValue()
    {
        var response = new GetRewardResponse(Guid.NewGuid().ToString(), "VIP Ticket", "VIP Entrance", "1500", "20", "VIP");

        response.Cost.Should().Be("1500");
    }
    #endregion

    #region QuantityAvailable
    [TestMethod]
    [TestCategory("Validation")]
    public void QuantityAvailable_Getter_ReturnsAssignedValue()
    {
        var response = new GetRewardResponse(Guid.NewGuid().ToString(), "VIP Ticket", "VIP Entrance", "1500", "20", "VIP");

        response.QuantityAvailable.Should().Be("20");
    }
    #endregion

    #region Membership
    [TestMethod]
    [TestCategory("Validation")]
    public void Membership_Getter_ReturnsAssignedValue()
    {
        var response = new GetRewardResponse(Guid.NewGuid().ToString(), "VIP Ticket", "VIP Entrance", "1500", "20", "VIP");

        response.Membership.Should().Be("VIP");
    }
    #endregion
}
