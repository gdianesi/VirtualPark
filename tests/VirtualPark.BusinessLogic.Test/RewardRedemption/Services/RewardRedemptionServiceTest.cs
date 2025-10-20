namespace VirtualPark.BusinessLogic.Test.RewardRedemption.Services;

[TestClass]
[TestCategory("Service")]
[TestCategory("RewardRedemptionService")]
public sealed class RewardRedemptionServiceTest
{
    [TestMethod]
    [TestCategory("Validation")]
    public void RedeemReward_WhenValid_ShouldCreateRedemption()
    {
        var rewardId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var reward = new Reward
        {
            Id = rewardId,
            Name = "VIP Ticket",
            Description = "Access to all rides",
            Cost = 200,
            QuantityAvailable = 5,
            RequiredMembershipLevel = Membership.Standard
        };

        var visitor = new VisitorProfile
        {
            Id = visitorId,
            Score = 500,
            Membership = Membership.Standard
        };

        var args = new RewardRedemptionArgs(
            rewardId.ToString(),
            visitorId.ToString(),
            "2025-10-19",
            reward.Cost.ToString());

        _rewardRepositoryMock
            .Setup(r => r.Get(rw => rw.Id == rewardId))
            .Returns(reward);

        _visitorRepositoryMock
            .Setup(v => v.Get(vs => vs.Id == visitorId))
            .Returns(visitor);

        _redemptionRepositoryMock
            .Setup(r => r.Add(It.Is<RewardRedemption>(rr =>
                rr.RewardId == rewardId &&
                rr.VisitorId == visitorId &&
                rr.PointsSpent == reward.Cost)));

        _rewardRepositoryMock
            .Setup(r => r.Update(It.Is<Reward>(rw =>
                rw.Id == rewardId &&
                rw.QuantityAvailable == 4)));

        _visitorRepositoryMock
            .Setup(v => v.Update(It.Is<VisitorProfile>(vp =>
                vp.Id == visitorId &&
                vp.Score == 300)));

        Guid result = _redemptionService.RedeemReward(args);

        result.Should().NotBeEmpty();

        _rewardRepositoryMock.VerifyAll();
        _visitorRepositoryMock.VerifyAll();
        _redemptionRepositoryMock.VerifyAll();
    }
}
