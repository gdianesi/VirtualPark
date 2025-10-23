using FluentAssertions;
using VirtualPark.BusinessLogic.RewardRedemptions.Models;

namespace VirtualPark.BusinessLogic.Test.RewardRedemptions.Models;

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
        var args = new RewardRedemptionArgs(rewardIdString, Guid.NewGuid().ToString(), "2025-12-19", "300");

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
            var rewardRedemptionArgs = new RewardRedemptionArgs(invalidRewardId, Guid.NewGuid().ToString(), "2025-12-19", "300");
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
        var args = new RewardRedemptionArgs(rewardId, visitorId, "2025-12-19", "300");

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
            var rewardRedemptionArgs = new RewardRedemptionArgs(Guid.NewGuid().ToString(), invalidVisitorId, "2025-12-19", "200");
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
        var args = new RewardRedemptionArgs(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "2025-12-19", "300");
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
            var rewardRedemptionArgs = new RewardRedemptionArgs(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), invalidDate, "200");
        };
        act.Should().Throw<ArgumentException>();
    }
    #endregion
    #endregion

    #region PointsSpend
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenPointsSpentIsValid_ShouldSetPointsSpent()
    {
        var args = new RewardRedemptionArgs(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "2025-12-19", "200");
        args.PointsSpent.Should().Be(200);
    }
    #endregion

    #region Failure
    [DataTestMethod]
    [DataRow("")]
    [DataRow("abc")]
    [DataRow(" ")]
    [TestCategory("Validation")]
    public void Constructor_WhenPointsSpentIsInvalid_ShouldThrowException(string invalidPoints)
    {
        Action act = () =>
        {
            var rewardRedemptionArgs = new RewardRedemptionArgs(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "2025-10-19", invalidPoints);
        };
        act.Should().Throw<Exception>();
    }
    #endregion
    #endregion
}
