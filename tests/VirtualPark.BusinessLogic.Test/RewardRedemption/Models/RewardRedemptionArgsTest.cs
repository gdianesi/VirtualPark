namespace VirtualPark.BusinessLogic.Test.RewardRedemption.Models;

[TestClass]
[TestCategory("Args")]
public sealed class RewardRedemptionArgsTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenRewardIdIsValid_ShouldSetRewardId()
    {
        var rewardId = Guid.NewGuid().ToString();
        var args = new RewardRedemptionArgs(rewardId);

        args.RewardId.Should().Be(Guid.Parse(rewardId));
    }
}
