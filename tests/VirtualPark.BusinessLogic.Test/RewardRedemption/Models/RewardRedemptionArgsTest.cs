using FluentAssertions;
using VirtualPark.BusinessLogic.RewardRedemptions.Models;

namespace VirtualPark.BusinessLogic.Test.RewardRedemption.Models;

[TestClass]
[TestCategory("Args")]
public sealed class RewardRedemptionArgsTest
{
    #region RewardId
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenRewardIdIsValid_ShouldSetRewardId()
    {
        var rewardId = Guid.NewGuid();
        var rewardIdString = rewardId.ToString();
        var args = new RewardRedemptionArgs(rewardIdString, Guid.NewGuid().ToString(), "2025-12-19");

        args.RewardId.Should().Be(rewardId);
    }
    #endregion
    #endregion

    #region Failure
    [DataTestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("invalid-guid")]
    [TestCategory("Validation")]
    public void Constructor_WhenRewardIdIsInvalid_ShouldThrowException(string invalidRewardId)
    {
        Action act = () =>
        {
            var rewardRedemptionArgs = new RewardRedemptionArgs(invalidRewardId, Guid.NewGuid().ToString(), "2025-12-19");
        };
        act.Should().Throw<Exception>();
    }
    #endregion

    #region VisitorId
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenVisitorIdIsValid_ShouldSetVisitorId()
    {
        var rewardId = Guid.NewGuid().ToString();
        var visitorId = Guid.NewGuid().ToString();
        var args = new RewardRedemptionArgs(rewardId, visitorId, "2025-12-19");

        args.VisitorId.Should().Be(Guid.Parse(visitorId));
    }

    #region Failure
    [DataTestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("invalid-guid")]
    [TestCategory("Validation")]
    public void Constructor_WhenVisitorIdIsInvalid_ShouldThrowException(string invalidVisitorId)
    {
        Action act = () =>
        {
            var rewardRedemptionArgs = new RewardRedemptionArgs(Guid.NewGuid().ToString(), invalidVisitorId, "2025-12-19");
        };
        act.Should().Throw<Exception>();
    }
    #endregion
    #endregion

    #region Date
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenDateIsValid_ShouldSetDate()
    {
        var args = new RewardRedemptionArgs(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "2025-12-19");
        args.Date.Should().Be(new DateOnly(2025, 12, 19));
    }
    #endregion

    #region Failure
    [DataTestMethod]
    [DataRow("not-a-date")]
    [DataRow("2025/10/19")]
    [TestCategory("Validation")]
    public void Constructor_WhenDateIsInvalid_ShouldThrowArgumentException(string invalidDate)
    {
        Action act = () =>
        {
            var rewardRedemptionArgs = new RewardRedemptionArgs(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), invalidDate);
        };
        act.Should().Throw<ArgumentException>();
    }
    #endregion
    #endregion

    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenPointsSpentIsValid_ShouldSetPointsSpent()
    {
        var args = new RewardRedemptionArgs(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "2025-10-19", "200");
        args.PointsSpent.Should().Be(200);
    }
}
