using FluentAssertions;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.Test.Reward.Entity;

using VirtualPark.BusinessLogic.Rewards.Entity;

[TestClass]
[TestCategory("Entity")]
#region Name
public sealed class RewardTest
{
    [TestMethod]
    [TestCategory("Success")]
    public void Constructor_WhenNameIsValid_ShouldSetName()
    {
        const string name = "VIP entrance";

        var reward = new Reward { Name = name };

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

        var reward = new Reward { Name = name, Description = description };

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

        var reward = new Reward { Name = name, Description = description, Cost = cost};

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

        var reward = new Reward { Name = name, Description = description, Cost = cost, QuantityAvailable = quantity};

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

        var reward = new Reward { Name = name, Description = description, Cost = cost, QuantityAvailable = quantity, RequiredMembershipLevel = level };

        reward.RequiredMembershipLevel.Should().Be(level);
    }
    #endregion
}
