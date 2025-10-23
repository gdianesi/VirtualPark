using FluentAssertions;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using RewardEntity = VirtualPark.BusinessLogic.Rewards.Entity.Reward;

namespace VirtualPark.BusinessLogic.Test.Rewards.Entity;

[TestClass]
[TestCategory("Entity")]
public sealed class RewardTest
{
    [TestMethod]
    [TestCategory("Success")]
    #region Name
    public void Constructor_WhenNameIsValid_ShouldSetName()
    {
        const string name = "VIP entrance";

        var reward = new RewardEntity { Name = name };

        reward.Name.Should().Be(name);
    }
    #endregion

    #region Description
    [TestMethod]
    [TestCategory("Success")]
    public void Constructor_WhenDescriptionIsValid_ShouldSetDescription()
    {
        const string name = "VIP entrance";
        const string description = "Exlusive access";

        var reward = new RewardEntity { Name = name, Description = description };

        reward.Description.Should().Be(description);
    }
    #endregion

    #region Cost
    [TestMethod]
    [TestCategory("Success")]
    public void Constructor_WhenCostIsValid_ShouldSetCost()
    {
        const string name = "VIP Ticket";
        const string description = "Access to all attractions";
        const int cost = 500;

        var reward = new RewardEntity { Name = name, Description = description, Cost = cost };

        reward.Cost.Should().Be(cost);
    }
    #endregion

    #region  QuantityAvailable
    [TestMethod]
    [TestCategory("Success")]
    public void Constructor_WhenQuantityAvailableIsValid_ShouldSetQuantityAvailable()
    {
        const string name = "VIP Ticket";
        const string description = "Access to all attractions";
        const int cost = 500;
        const int quantity = 10;

        var reward = new RewardEntity { Name = name, Description = description, Cost = cost, QuantityAvailable = quantity };

        reward.QuantityAvailable.Should().Be(quantity);
    }
    #endregion

    #region RequiredMembershipLevel
    [TestMethod]
    [TestCategory("Success")]
    public void Constructor_WhenRequiredMembershipLevelIsValid_ShouldSetRequiredMembershipLevel()
    {
        const string name = "VIP Ticket";
        const string description = "Access to all attractions";
        const int cost = 500;
        const int quantity = 10;
        const Membership level = Membership.VIP;

        var reward = new RewardEntity { Name = name, Description = description, Cost = cost, QuantityAvailable = quantity, RequiredMembershipLevel = level };

        reward.RequiredMembershipLevel.Should().Be(level);
    }
    #endregion

    #region Id
    public void Constructor_WhenCreateReward_ShouldSetGuid()
    {
        var reward = new RewardEntity();

        reward.Name.Should().NotBe(Guid.Empty.ToString());
    }
    #endregion
}
