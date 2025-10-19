using FluentAssertions;

namespace VirtualPark.BusinessLogic.Test.Reward.Entity;

using VirtualPark.BusinessLogic.Rewards.Entity;

#region Name
[TestClass]
[TestCategory("Entity")]
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
}
#endregion
