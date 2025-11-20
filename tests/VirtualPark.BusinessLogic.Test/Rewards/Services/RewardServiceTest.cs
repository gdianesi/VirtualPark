using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Rewards.Entity;
using VirtualPark.BusinessLogic.Rewards.Models;
using VirtualPark.BusinessLogic.Rewards.Service;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Rewards.Services;

[TestClass]
[TestCategory("Service")]
[TestCategory("RewardService")]
public sealed class RewardServiceTest
{
    private Mock<IRepository<BusinessLogic.Rewards.Entity.Reward>> _rewardRepositoryMock = null!;
    private RewardService _rewardService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _rewardRepositoryMock = new Mock<IRepository<BusinessLogic.Rewards.Entity.Reward>>(MockBehavior.Strict);
        _rewardService = new RewardService(_rewardRepositoryMock.Object);
    }

    #region Create
    [TestMethod]
    [TestCategory("Validation")]
    public void Create_WhenArgsAreValid_ShouldReturnRewardId()
    {
        var args = new RewardArgs("VIP Ticket", "Access to all rides", "200", "5", "Standard");

        _rewardRepositoryMock
            .Setup(r => r.Add(It.Is<BusinessLogic.Rewards.Entity.Reward>(rw =>
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
        var reward = new BusinessLogic.Rewards.Entity.Reward
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
            .Returns((BusinessLogic.Rewards.Entity.Reward?)null);

        Action act = () => _rewardService.Get(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Reward with id {id} not found.");

        _rewardRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region GetAll
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_WhenRewardsExist_ShouldReturnListOfRewards()
    {
        var rewardsFromRepo = new List<BusinessLogic.Rewards.Entity.Reward>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "VIP Ticket",
                Description = "Access to all rides",
                Cost = 200,
                QuantityAvailable = 5,
                RequiredMembershipLevel = Membership.Standard
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Premium Pass",
                Description = "Fast pass for attractions",
                Cost = 400,
                QuantityAvailable = 3,
                RequiredMembershipLevel = Membership.Premium
            }
        };

        _rewardRepositoryMock
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<BusinessLogic.Rewards.Entity.Reward, bool>>>()))
            .Returns(rewardsFromRepo);

        var result = _rewardService.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("VIP Ticket");
        result[1].RequiredMembershipLevel.Should().Be(Membership.Premium);

        _rewardRepositoryMock.VerifyAll();
    }
    #endregion
    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_WhenRepositoryReturnsEmptyList_ShouldThrowInvalidOperationException()
    {
        _rewardRepositoryMock
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<BusinessLogic.Rewards.Entity.Reward, bool>>>()))
            .Returns([]);

        Action act = () => _rewardService.GetAll();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("There are no active rewards.");

        _rewardRepositoryMock.VerifyAll();
    }

    #endregion
    #endregion

    #region Remove
    #region Success
    [TestMethod]
    public void Remove_WhenRewardExists_ShouldSetQuantityToZeroAndCallUpdate()
    {
        var id = Guid.NewGuid();
        var reward = new Reward { Id = id, QuantityAvailable = 5 };

        _rewardRepositoryMock
            .Setup(r => r.Get(rw => rw.Id == id))
            .Returns(reward);

        _rewardRepositoryMock
            .Setup(r => r.Update(It.Is<Reward>(rw =>
                rw.Id == id &&
                rw.QuantityAvailable == 0)))
            .Verifiable();

        _rewardService.Remove(id);

        reward.QuantityAvailable.Should().Be(0);
        _rewardRepositoryMock.VerifyAll();
    }
    #endregion
    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Remove_WhenRewardDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var id = Guid.NewGuid();

        _rewardRepositoryMock
            .Setup(r => r.Get(rw => rw.Id == id))
            .Returns((BusinessLogic.Rewards.Entity.Reward?)null);

        Action act = () => _rewardService.Remove(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Reward with id {id} not found.");

        _rewardRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region Update
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Update_WhenRewardExists_ShouldUpdateFields()
    {
        var id = Guid.NewGuid();

        var existingReward = new BusinessLogic.Rewards.Entity.Reward
        {
            Id = id,
            Name = "Old Name",
            Description = "Old Desc",
            Cost = 100,
            QuantityAvailable = 10,
            RequiredMembershipLevel = Membership.Standard
        };

        var args = new RewardArgs("New Name", "New Desc", "200", "5", "Premium");

        _rewardRepositoryMock
            .Setup(r => r.Get(rw => rw.Id == id))
            .Returns(existingReward);

        _rewardRepositoryMock
            .Setup(r => r.Update(It.Is<BusinessLogic.Rewards.Entity.Reward>(rw =>
                rw.Id == id &&
                rw.Name == args.Name &&
                rw.Description == args.Description &&
                rw.Cost == args.Cost &&
                rw.QuantityAvailable == args.QuantityAvailable &&
                rw.RequiredMembershipLevel == Membership.Premium)));

        _rewardService.Update(args, id);

        _rewardRepositoryMock.VerifyAll();
    }
    #endregion
    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Update_WhenRewardDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var id = Guid.NewGuid();
        var args = new RewardArgs("New Name", "New Desc", "200", "5", "VIP");

        _rewardRepositoryMock
            .Setup(r => r.Get(rw => rw.Id == id))
            .Returns((BusinessLogic.Rewards.Entity.Reward?)null);

        Action act = () => _rewardService.Update(args, id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Reward with id {id} not found.");

        _rewardRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region GetDeleted

    [TestMethod]
    [TestCategory("Validation")]
    public void GetDeleted_WhenDeletedRewardsExist_ShouldReturnOnlyDeletedRewards()
    {
        var deleted1 = new BusinessLogic.Rewards.Entity.Reward
        {
            Id = Guid.NewGuid(),
            Name = "Deleted 1",
            Description = "Test",
            Cost = 100,
            QuantityAvailable = 0,
            RequiredMembershipLevel = Membership.Standard
        };

        var deleted2 = new BusinessLogic.Rewards.Entity.Reward
        {
            Id = Guid.NewGuid(),
            Name = "Deleted 2",
            Description = "Test",
            Cost = 150,
            QuantityAvailable = 0,
            RequiredMembershipLevel = Membership.Premium
        };

        var list = new List<BusinessLogic.Rewards.Entity.Reward> { deleted1, deleted2 };

        _rewardRepositoryMock
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<BusinessLogic.Rewards.Entity.Reward, bool>>>()))
            .Returns(list);

        var result = _rewardService.GetDeleted();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].QuantityAvailable.Should().Be(0);
        result[1].QuantityAvailable.Should().Be(0);

        _rewardRepositoryMock.VerifyAll();
    }
    #endregion

    #region Restore

    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Restore_WhenRewardExists_ShouldUpdateQuantityCorrectly()
    {
        var id = Guid.NewGuid();

        var reward = new BusinessLogic.Rewards.Entity.Reward
        {
            Id = id,
            Name = "Deleted Reward",
            Description = "Test",
            Cost = 100,
            QuantityAvailable = 0,
            RequiredMembershipLevel = Membership.Standard
        };

        _rewardRepositoryMock
            .Setup(r => r.Get(rw => rw.Id == id))
            .Returns(reward);

        _rewardRepositoryMock
            .Setup(r => r.Update(It.Is<BusinessLogic.Rewards.Entity.Reward>(rw =>
                rw.Id == id &&
                rw.QuantityAvailable == 10)))
            .Verifiable();

        _rewardService.Restore(id, 10);

        reward.QuantityAvailable.Should().Be(10);

        _rewardRepositoryMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Restore_WhenRewardDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var id = Guid.NewGuid();

        _rewardRepositoryMock
            .Setup(r => r.Get(rw => rw.Id == id))
            .Returns((BusinessLogic.Rewards.Entity.Reward?)null);

        Action act = () => _rewardService.Restore(id, 10);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Reward with id {id} not found.");

        _rewardRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion
}
