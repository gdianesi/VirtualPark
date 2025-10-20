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
        var args = new RewardRedemptionArgs(rewardIdString);

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
            var rewardRedemptionArgs = new RewardRedemptionArgs(invalidRewardId);
        };
        act.Should().Throw<Exception>();
    }
    #endregion
}
