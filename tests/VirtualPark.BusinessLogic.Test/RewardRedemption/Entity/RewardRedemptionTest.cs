using FluentAssertions;

namespace VirtualPark.BusinessLogic.Test.RewardRedemption.Entity;
using VirtualPark.BusinessLogic.RewardRedemption.Entity;

[TestClass]
[TestCategory("Entity")]
public sealed class RewardRedemptionTest
{
    #region RewardId
    [TestMethod]
    [TestCategory("Success")]
    public void Constructor_WhenRewardIdIsValid_ShouldSetRewardId()
    {
        var rewardId = Guid.NewGuid();

        var redemption = new RewardRedemption { RewardId = rewardId };

        redemption.RewardId.Should().Be(rewardId);
    }
    #endregion

    #region  VisitorId
    [TestMethod]
    [TestCategory("Success")]
    public void Constructor_WhenVisitorIdIsValid_ShouldSetVisitorId()
    {
        var rewardId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var redemption = new RewardRedemption { RewardId = rewardId, VisitorId = visitorId };

        redemption.VisitorId.Should().Be(visitorId);
    }
    #endregion

    #region Date
    [TestMethod]
    [TestCategory("Success")]
    public void Constructor_WhenDateIsValid_ShouldSetDate()
    {
        var rewardId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();
        var date = new DateOnly(2025, 12, 19);

        var redemption = new RewardRedemption { RewardId = rewardId, VisitorId = visitorId, Date = date };

        redemption.Date.Should().Be(date);
    }
    #endregion
}
