using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.RewardRedemptions.Models;
using VirtualPark.BusinessLogic.RewardRedemptions.Service;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.RewardRedemption.Services;

[TestClass]
[TestCategory("Service")]
[TestCategory("RewardRedemptionService")]
public sealed class RewardRedemptionServiceTest
{
    private Mock<IRepository<Rewards.Entity.Reward>> _rewardRepositoryMock = null!;
    private Mock<IRepository<RewardRedemptions.Entity.RewardRedemption>> _redemptionRepositoryMock = null!;
    private Mock<IRepository<VisitorProfile>> _visitorRepositoryMock = null!;
    private RewardRedemptionService _redemptionService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _rewardRepositoryMock = new Mock<IRepository<Rewards.Entity.Reward>>(MockBehavior.Strict);
        _redemptionRepositoryMock = new Mock<IRepository<RewardRedemptions.Entity.RewardRedemption>>(MockBehavior.Strict);
        _visitorRepositoryMock = new Mock<IRepository<VisitorProfile>>(MockBehavior.Strict);

        _redemptionService = new RewardRedemptionService(
            _rewardRepositoryMock.Object,
            _redemptionRepositoryMock.Object,
            _visitorRepositoryMock.Object);
    }

    #region ReedemReward
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void RedeemReward_WhenValid_ShouldCreateRedemption()
    {
        var rewardId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var reward = new Rewards.Entity.Reward
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
            "2025-12-19",
            reward.Cost.ToString());

        _rewardRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Rewards.Entity.Reward, bool>>>()))
            .Returns(reward);

        _visitorRepositoryMock
            .Setup(v => v.Get(It.IsAny<Expression<Func<VisitorProfile, bool>>>()))
            .Returns(visitor);

        _redemptionRepositoryMock
            .Setup(r => r.Add(It.Is<RewardRedemptions.Entity.RewardRedemption>(rr =>
                rr.RewardId == rewardId &&
                rr.VisitorId == visitorId &&
                rr.PointsSpent == reward.Cost)));

        _rewardRepositoryMock
            .Setup(r => r.Update(It.Is<Rewards.Entity.Reward>(rw =>
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
    #endregion
    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void RedeemReward_WhenVisitorHasNotEnoughPoints_ShouldThrowInvalidOperationException()
    {
        var rewardId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var reward = new Rewards.Entity.Reward
        {
            Id = rewardId,
            Name = "VIP Ticket",
            Description = "Access to all rides",
            Cost = 500,
            QuantityAvailable = 5,
            RequiredMembershipLevel = Membership.Standard
        };

        var visitor = new VisitorProfile
        {
            Id = visitorId,
            Score = 200,
            Membership = Membership.Standard
        };

        var args = new RewardRedemptionArgs(
            rewardId.ToString(),
            visitorId.ToString(),
            "2025-12-19",
            reward.Cost.ToString());

        _rewardRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Rewards.Entity.Reward, bool>>>()))
            .Returns(reward);

        _visitorRepositoryMock
            .Setup(v => v.Get(It.IsAny<Expression<Func<VisitorProfile, bool>>>()))
            .Returns(visitor);

        Action act = () => _redemptionService.RedeemReward(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Visitor does not have enough points to redeem this reward.");

        _rewardRepositoryMock.VerifyAll();
        _visitorRepositoryMock.VerifyAll();
    }
    #endregion
    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void RedeemReward_WhenVisitorDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var rewardId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var reward = new Rewards.Entity.Reward
        {
            Id = rewardId,
            Name = "VIP Ticket",
            Description = "Access to all rides",
            Cost = 200,
            QuantityAvailable = 5,
            RequiredMembershipLevel = Membership.Standard
        };

        var args = new RewardRedemptionArgs(
            rewardId.ToString(),
            visitorId.ToString(),
            "2025-12-19",
            reward.Cost.ToString());

        _rewardRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Rewards.Entity.Reward, bool>>>()))
            .Returns(reward);

        _visitorRepositoryMock
            .Setup(v => v.Get(It.IsAny<Expression<Func<VisitorProfile, bool>>>()))
            .Returns((VisitorProfile?)null);

        Action act = () => _redemptionService.RedeemReward(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Visitor with id {visitorId} not found.");

        _rewardRepositoryMock.VerifyAll();
        _visitorRepositoryMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void RedeemReward_WhenRewardDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var rewardId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var args = new RewardRedemptionArgs(
            rewardId.ToString(),
            visitorId.ToString(),
            "2025-12-19",
            "100");

        _rewardRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Rewards.Entity.Reward, bool>>>()))
            .Returns((Rewards.Entity.Reward?)null);

        _visitorRepositoryMock
            .Setup(v => v.Get(It.IsAny<Expression<Func<VisitorProfile, bool>>>()))
            .Returns(new VisitorProfile { Id = visitorId, Score = 300 });

        Action act = () => _redemptionService.RedeemReward(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Reward with id {rewardId} not found.");

        _rewardRepositoryMock.VerifyAll();
    }
    #endregion
    [TestMethod]
    [TestCategory("Validation")]
    public void RedeemReward_WhenRewardIsNotAvailable_ShouldThrowInvalidOperationException()
    {
        var rewardId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var reward = new Rewards.Entity.Reward
        {
            Id = rewardId,
            Name = "Premium Pass",
            Description = "Fast pass for attractions",
            Cost = 300,
            QuantityAvailable = 0,
            RequiredMembershipLevel = Membership.Standard
        };

        var visitor = new VisitorProfile
        {
            Id = visitorId,
            Score = 1000,
            Membership = Membership.Standard
        };

        var args = new RewardRedemptionArgs(
            rewardId.ToString(),
            visitorId.ToString(),
            "2025-12-19",
            reward.Cost.ToString());

        _rewardRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Rewards.Entity.Reward, bool>>>()))
            .Returns(reward);

        _visitorRepositoryMock
            .Setup(v => v.Get(It.IsAny<Expression<Func<VisitorProfile, bool>>>()))
            .Returns(visitor);

        Action act = () => _redemptionService.RedeemReward(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Reward is not available.");

        _rewardRepositoryMock.VerifyAll();
        _visitorRepositoryMock.VerifyAll();
    }

    #endregion
}
