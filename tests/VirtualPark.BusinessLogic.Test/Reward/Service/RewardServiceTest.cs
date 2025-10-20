namespace VirtualPark.BusinessLogic.Test.Reward.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("RewardService")]
public sealed class RewardServiceTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void Create_WhenArgsAreValid_ShouldReturnRewardId()
    {
        var args = new RewardArgs("VIP Ticket", "Access to all rides", "200", "5", "Standard");

        _rewardRepositoryMock
            .Setup(r => r.Add(It.Is<Reward>(rw =>
                rw.Name == args.Name &&
                rw.Description == args.Description &&
                rw.Cost == args.Cost &&
                rw.QuantityAvailable == args.QuantityAvailable &&
                rw.RequiredMembershipLevel.ToString() == args.RequiredMembershipLevel)));

        Guid result = _rewardService.Create(args);

        result.Should().NotBeEmpty();

        _rewardRepositoryMock.VerifyAll();
    }
}
