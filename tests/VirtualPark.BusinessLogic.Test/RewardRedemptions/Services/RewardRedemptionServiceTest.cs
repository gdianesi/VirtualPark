using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.RewardRedemptions.Models;
using VirtualPark.BusinessLogic.RewardRedemptions.Service;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.Repository;
using RewardEntity = VirtualPark.BusinessLogic.Rewards.Entity.Reward;

namespace VirtualPark.BusinessLogic.Test.RewardRedemptions.Services;

[TestClass]
[TestCategory("Service")]
[TestCategory("RewardRedemptionService")]
public sealed class RewardRedemptionServiceTest
{
    private Mock<IRepository<BusinessLogic.RewardRedemptions.Entity.RewardRedemption>> _redemptionRepositoryMock = null!;
    private RewardRedemptionService _redemptionService = null!;
    private Mock<IRepository<RewardEntity>> _rewardRepositoryMock = null!;
    private Mock<IRepository<VisitorProfile>> _visitorRepositoryMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _rewardRepositoryMock = new Mock<IRepository<RewardEntity>>(MockBehavior.Strict);
        _redemptionRepositoryMock =
            new Mock<IRepository<BusinessLogic.RewardRedemptions.Entity.RewardRedemption>>(MockBehavior.Strict);
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

        var reward = new RewardEntity
        {
            Id = rewardId,
            Name = "VIP Ticket",
            Description = "Access to all rides",
            Cost = 200,
            QuantityAvailable = 5,
            RequiredMembershipLevel = Membership.Standard
        };

        var visitor = new VisitorProfile { Id = visitorId, Score = 500, Membership = Membership.Standard };

        var args = new RewardRedemptionArgs(
            rewardId.ToString(),
            visitorId.ToString(),
            "2025-12-19",
            reward.Cost.ToString());

        _rewardRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<RewardEntity, bool>>>()))
            .Returns(reward);

        _visitorRepositoryMock
            .Setup(v => v.Get(It.IsAny<Expression<Func<VisitorProfile, bool>>>()))
            .Returns(visitor);

        _redemptionRepositoryMock
            .Setup(r => r.Add(It.Is<BusinessLogic.RewardRedemptions.Entity.RewardRedemption>(rr =>
                rr.RewardId == rewardId &&
                rr.VisitorId == visitorId &&
                rr.PointsSpent == reward.Cost)));

        _rewardRepositoryMock
            .Setup(r => r.Update(It.Is<RewardEntity>(rw =>
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

        var reward = new RewardEntity
        {
            Id = rewardId,
            Name = "VIP Ticket",
            Description = "Access to all rides",
            Cost = 500,
            QuantityAvailable = 5,
            RequiredMembershipLevel = Membership.Standard
        };

        var visitor = new VisitorProfile { Id = visitorId, Score = 200, Membership = Membership.Standard };

        var args = new RewardRedemptionArgs(
            rewardId.ToString(),
            visitorId.ToString(),
            "2025-12-19",
            reward.Cost.ToString());

        _rewardRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<RewardEntity, bool>>>()))
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

        var reward = new RewardEntity
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
            .Setup(r => r.Get(It.IsAny<Expression<Func<RewardEntity, bool>>>()))
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
            .Setup(r => r.Get(It.IsAny<Expression<Func<RewardEntity, bool>>>()))
            .Returns((RewardEntity?)null);

        _visitorRepositoryMock
            .Setup(v => v.Get(It.IsAny<Expression<Func<VisitorProfile, bool>>>()))
            .Returns(new VisitorProfile { Id = visitorId, Score = 300 });

        Action act = () => _redemptionService.RedeemReward(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Reward with id {rewardId} not found.");

        _rewardRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region GetAll

    #region Success

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_WhenRedemptionsExist_ShouldReturnList()
    {
        var visitorId = Guid.NewGuid();
        var rewardId = Guid.NewGuid();

        var redemptions = new List<BusinessLogic.RewardRedemptions.Entity.RewardRedemption>
        {
            new()
            {
                RewardId = rewardId, VisitorId = visitorId, Date = new DateOnly(2025, 12, 20), PointsSpent = 200
            },
            new()
            {
                RewardId = Guid.NewGuid(),
                VisitorId = visitorId,
                Date = new DateOnly(2025, 12, 21),
                PointsSpent = 300
            }
        };

        _redemptionRepositoryMock
            .Setup(r => r.GetAll())
            .Returns(redemptions);

        List<BusinessLogic.RewardRedemptions.Entity.RewardRedemption> result = _redemptionService.GetAll();

        result.Should().HaveCount(2);
        result[0].VisitorId.Should().Be(visitorId);

        _redemptionRepositoryMock.VerifyAll();
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_WhenNoRedemptionsExist_ShouldThrowInvalidOperationException()
    {
        _redemptionRepositoryMock
            .Setup(r => r.GetAll())
            .Returns((List<BusinessLogic.RewardRedemptions.Entity.RewardRedemption>?)null);

        Action act = () => _redemptionService.GetAll();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("There are no reward redemptions registered.");

        _redemptionRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region GetByVisitor

    #region Success

    [TestMethod]
    [TestCategory("Validation")]
    public void GetByVisitor_WhenRedemptionsExist_ShouldReturnList()
    {
        var visitorId = Guid.NewGuid();
        var rewardId = Guid.NewGuid();

        var redemptions = new List<BusinessLogic.RewardRedemptions.Entity.RewardRedemption>
        {
            new()
            {
                RewardId = rewardId, VisitorId = visitorId, Date = new DateOnly(2025, 12, 19), PointsSpent = 150
            }
        };

        _redemptionRepositoryMock
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<BusinessLogic.RewardRedemptions.Entity.RewardRedemption, bool>>>()))
            .Returns(redemptions);

        List<BusinessLogic.RewardRedemptions.Entity.RewardRedemption> result = _redemptionService.GetByVisitor(visitorId);

        result.Should().HaveCount(1);
        result[0].VisitorId.Should().Be(visitorId);

        _redemptionRepositoryMock.VerifyAll();
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void GetByVisitor_WhenNoRedemptionsExist_ShouldThrowInvalidOperationException()
    {
        var visitorId = Guid.NewGuid();

        _redemptionRepositoryMock
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<BusinessLogic.RewardRedemptions.Entity.RewardRedemption, bool>>>()))
            .Returns([]);

        Action act = () => _redemptionService.GetByVisitor(visitorId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Visitor with id {visitorId} has no redemptions.");

        _redemptionRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region Get
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Get_WhenRedemptionExists_ShouldReturnRedemption()
    {
        var visitorId = Guid.NewGuid();
        var rewardId = Guid.NewGuid();

        var redemption = new BusinessLogic.RewardRedemptions.Entity.RewardRedemption
        {
            RewardId = rewardId,
            VisitorId = visitorId,
            Date = new DateOnly(2025, 12, 19),
            PointsSpent = 200
        };

        _redemptionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<BusinessLogic.RewardRedemptions.Entity.RewardRedemption, bool>>>()))
            .Returns(redemption);

        BusinessLogic.RewardRedemptions.Entity.RewardRedemption result = _redemptionService.Get(redemption.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(redemption.Id);
        result.VisitorId.Should().Be(visitorId);
        result.RewardId.Should().Be(rewardId);
        result.PointsSpent.Should().Be(200);

        _redemptionRepositoryMock.VerifyAll();
    }
    #endregion
    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Get_WhenRedemptionDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var id = Guid.NewGuid();

        _redemptionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<BusinessLogic.RewardRedemptions.Entity.RewardRedemption, bool>>>()))
            .Returns((BusinessLogic.RewardRedemptions.Entity.RewardRedemption?)null);

        Action act = () => _redemptionService.Get(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Reward redemption with id {id} not found.");

        _redemptionRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion
}
