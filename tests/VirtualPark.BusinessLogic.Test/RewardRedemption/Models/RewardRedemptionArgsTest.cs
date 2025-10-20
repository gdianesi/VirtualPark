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
        var args = new RewardRedemptionArgs(rewardIdString, Guid.NewGuid().ToString());

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
            var rewardRedemptionArgs = new RewardRedemptionArgs(invalidRewardId, Guid.NewGuid().ToString());
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
        var args = new RewardRedemptionArgs(rewardId, visitorId);

        args.VisitorId.Should().Be(Guid.Parse(visitorId));
    }
    #endregion

    [DataTestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("invalid-guid")]
    [TestCategory("Validation")]
    public void Constructor_WhenVisitorIdIsInvalid_ShouldThrowException(string invalidVisitorId)
    {
        Action act = () =>
        {
            var rewardRedemptionArgs = new RewardRedemptionArgs(Guid.NewGuid().ToString(), invalidVisitorId);
        };
        act.Should().Throw<Exception>();
    }
}
