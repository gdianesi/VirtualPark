using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Rewards.Models;
using VirtualPark.BusinessLogic.Rewards.Service;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Reward.Service;
using VirtualPark.BusinessLogic.Rewards.Entity;

[TestClass]
[TestCategory("Service")]
[TestCategory("RewardService")]
public sealed class RewardServiceTest
{
    private Mock<IRepository<Reward>> _rewardRepositoryMock = null!;
    private RewardService _rewardService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _rewardRepositoryMock = new Mock<IRepository<Reward>>(MockBehavior.Strict);
        _rewardService = new RewardService(_rewardRepositoryMock.Object);
    }

    #region Create
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
                rw.RequiredMembershipLevel == args.RequiredMembershipLevel)));

        Guid result = _rewardService.Create(args);

        result.Should().NotBeEmpty();

        _rewardRepositoryMock.VerifyAll();
    }
    #endregion

    #region Get
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Get_WhenRewardExists_ShouldReturnReward()
    {
        var id = Guid.NewGuid();
        var reward = new Reward
        {
            Id = id,
            Name = "VIP Ticket",
            Description = "Access to all rides",
            Cost = 200,
            QuantityAvailable = 5,
            RequiredMembershipLevel = Membership.Standard
        };

        _rewardRepositoryMock
            .Setup(r => r.Get(rw => rw.Id == id))
            .Returns(reward);

        var result = _rewardService.Get(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Name.Should().Be("VIP Ticket");
        result.Description.Should().Be("Access to all rides");
        result.Cost.Should().Be(200);
        result.QuantityAvailable.Should().Be(5);
        result.RequiredMembershipLevel.Should().Be(Membership.Standard);

        _rewardRepositoryMock.VerifyAll();
    }
    #endregion
    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Get_WhenRewardDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var id = Guid.NewGuid();

        _rewardRepositoryMock
            .Setup(r => r.Get(rw => rw.Id == id))
            .Returns((Reward?)null);

        Action act = () => _rewardService.Get(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Reward with id {id} not found.");

        _rewardRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion
}
