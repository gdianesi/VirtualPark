using FluentAssertions;
using VirtualPark.BusinessLogic.Rewards.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.WebApi.Controllers.Reward.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Rewards.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetRewardResponse")]
public sealed class GetRewardResponseTest
{
    private static Reward BuildEntity(
        Guid? id = null,
        string? name = null,
        string? description = null,
        int? cost = null,
        int? quantity = null,
        Membership? membership = null)
    {
        return new Reward
        {
            Id = id ?? Guid.NewGuid(),
            Name = name ?? "VIP Ticket",
            Description = description ?? "VIP Entrance",
            Cost = cost ?? 1500,
            QuantityAvailable = quantity ?? 20,
            RequiredMembershipLevel = membership ?? Membership.Standard
        };
    }

    #region Id
    [TestMethod]
    public void Id_ShouldMapCorrectly()
    {
        var id = Guid.NewGuid();
        var entity = BuildEntity(id: id);

        var dto = new GetRewardResponse(entity);

        dto.Id.Should().Be(id.ToString());
    }
    #endregion

    #region Name
    [TestMethod]
    public void Name_ShouldMapCorrectly()
    {
        var entity = BuildEntity(name: "Premium Ticket");

        var dto = new GetRewardResponse(entity);

        dto.Name.Should().Be("Premium Ticket");
    }
    #endregion

    #region Description
    [TestMethod]
    public void Description_ShouldMapCorrectly()
    {
        var entity = BuildEntity(description: "Fast pass");

        var dto = new GetRewardResponse(entity);

        dto.Description.Should().Be("Fast pass");
    }
    #endregion

    #region Cost
    [TestMethod]
    public void Cost_ShouldMapCorrectly()
    {
        var entity = BuildEntity(cost: 999);

        var dto = new GetRewardResponse(entity);

        dto.Cost.Should().Be("999");
    }
    #endregion

    #region QuantityAvailable
    [TestMethod]
    public void QuantityAvailable_ShouldMapCorrectly()
    {
        var entity = BuildEntity(quantity: 5);

        var dto = new GetRewardResponse(entity);

        dto.QuantityAvailable.Should().Be("5");
    }
    #endregion

    #region Membership
    [TestMethod]
    public void Membership_ShouldMapCorrectly()
    {
        var entity = BuildEntity(membership: Membership.VIP);

        var dto = new GetRewardResponse(entity);

        dto.Membership.Should().Be("VIP");
    }
    #endregion
}
