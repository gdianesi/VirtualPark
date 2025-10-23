using FluentAssertions;
using VirtualPark.WebApi.Controllers.Reward.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Rewards.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateRewardRequest")]
public class CreateRewardRequestTest
{
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var request = new CreateRewardRequest { Name = "VIP Ticket" };
        request.Name.Should().Be("VIP Ticket");
    }
    #endregion

    #region Description
    [TestMethod]
    [TestCategory("Validation")]
    public void Description_Getter_ReturnsAssignedValue()
    {
        var request = new CreateRewardRequest { Description = "VIP entry with priority access" };
        request.Description.Should().Be("VIP entry with priority access");
    }
    #endregion

    #region PointsRequired
    [TestMethod]
    [TestCategory("Validation")]
    public void PointsRequired_Getter_ReturnsAssignedValue()
    {
        var request = new CreateRewardRequest { Cost = "1500" };
        request.Cost.Should().Be("1500");
    }
    #endregion

    #region QuantityAvailable
    [TestMethod]
    [TestCategory("Validation")]
    public void QuantityAvailable_Getter_ReturnsAssignedValue()
    {
        var request = new CreateRewardRequest { QuantityAvailable = "25" };
        request.QuantityAvailable.Should().Be("25");
    }
    #endregion

    #region Membership
    [TestMethod]
    [TestCategory("Validation")]
    public void Membership_Getter_ReturnsAssignedValue()
    {
        var request = new CreateRewardRequest { Membership = "VIP" };
        request.Membership.Should().Be("VIP");
    }
    #endregion

    #region ToArgs
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldMapAllFields_WhenAllAreValid()
    {
        var request = new CreateRewardRequest
        {
            Name = "VIP Ticket",
            Description = "VIP entry with priority access",
            Cost = "1500",
            QuantityAvailable = "20",
            Membership = "VIP"
        };

        var args = request.ToArgs();

        args.Should().NotBeNull();
        args.Name.Should().Be("VIP Ticket");
        args.Description.Should().Be("VIP entry with priority access");
        args.Cost.Should().Be(1500);
        args.QuantityAvailable.Should().Be(20);
        args.RequiredMembershipLevel.ToString().Should().Be("VIP");
    }
    #endregion
}
