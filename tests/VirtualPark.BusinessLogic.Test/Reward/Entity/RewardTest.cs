using FluentAssertions;

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
        const string name = "Entrada VIP";

        var reward = new Reward { Name = name };

        reward.Name.Should().Be(name);
    }
    #endregion
    [TestMethod]
    [TestCategory("Success")]
    public void Constructor_WhenDescriptionIsValid_ShouldSetDescription()
    {
        const string name = "Entrada VIP";
        const string description = "Acceso exclusivo a todas las atracciones sin filas";

        var reward = new Reward { Name = name, Description = description };

        reward.Description.Should().Be(description);
    }
}

