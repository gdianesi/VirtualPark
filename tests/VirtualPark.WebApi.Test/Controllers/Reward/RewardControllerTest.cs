using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Rewards.Models;
using VirtualPark.BusinessLogic.Rewards.Service;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.WebApi.Controllers.Reward;
using VirtualPark.WebApi.Controllers.Reward.ModelsIn;
using VirtualPark.WebApi.Controllers.Reward.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Reward;
using BusinessLogic.Rewards.Entity;

[TestClass]
public sealed class RewardControllerTest
{
    private RewardController _rewardController = null!;
    private Mock<IRewardService> _rewardServiceMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _rewardServiceMock = new Mock<IRewardService>(MockBehavior.Strict);
        _rewardController = new RewardController(_rewardServiceMock.Object);
    }

    #region Create

    [TestMethod]
    public void CreateReward_ValidInput_ReturnsCreatedRewardResponse()
    {
        var returnId = Guid.NewGuid();

        var request = new CreateRewardRequest
        {
            Name = "VIP Ticket",
            Description = "Priority Access",
            Cost = "1500",
            QuantityAvailable = "25",
            Membership = "VIP"
        };

        RewardArgs expectedArgs = request.ToArgs();

        _rewardServiceMock
            .Setup(s => s.Create(It.Is<RewardArgs>(a =>
                a.Name == expectedArgs.Name &&
                a.Description == expectedArgs.Description &&
                a.Cost == expectedArgs.Cost &&
                a.QuantityAvailable == expectedArgs.QuantityAvailable &&
                a.RequiredMembershipLevel == expectedArgs.RequiredMembershipLevel)))
            .Returns(returnId);

        CreateRewardResponse response = _rewardController.CreateReward(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<CreateRewardResponse>();
        response.Id.Should().Be(returnId.ToString());

        _rewardServiceMock.VerifyAll();
    }

    #endregion

    #region GetRewardById
    [TestMethod]
    public void GetRewardById_ValidInput_ReturnsGetRewardResponse()
    {
        var reward = new Reward
        {
            Name = "VIP Ticket",
            Description = "Priority Access",
            Cost = 1500,
            QuantityAvailable = 25,
            RequiredMembershipLevel = Membership.VIP
        };

        var id = reward.Id;

        _rewardServiceMock
            .Setup(s => s.Get(id))
            .Returns(reward);

        var response = _rewardController.GetRewardById(id.ToString());

        response.Should().NotBeNull();
        response.Should().BeOfType<GetRewardResponse>();
        response.Id.Should().Be(id.ToString());
        response.Name.Should().Be("VIP Ticket");
        response.Description.Should().Be("Priority Access");
        response.Cost.Should().Be("1500");
        response.QuantityAvailable.Should().Be("25");
        response.Membership.Should().Be("VIP");

        _rewardServiceMock.VerifyAll();
    }
    #endregion

    #region GetAll
    [TestMethod]
    public void GetAllRewards_ShouldReturnMappedList()
    {
        var reward1 = new Reward
        {
            Name = "VIP Ticket",
            Description = "Entrada VIP",
            Cost = 1500,
            QuantityAvailable = 25,
            RequiredMembershipLevel = Membership.Premium
        };

        var reward2 = new Reward
        {
            Name = "Souvenir Pack",
            Description = "Bolsa de regalos",
            Cost = 500,
            QuantityAvailable = 100,
            RequiredMembershipLevel = Membership.Standard
        };

        var rewards = new List<Reward> { reward1, reward2 };

        _rewardServiceMock
            .Setup(s => s.GetAll())
            .Returns(rewards);

        var result = _rewardController.GetAllRewards();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var first = result.First();
        first.Name.Should().Be("VIP Ticket");
        first.Cost.Should().Be("1500");
        first.Membership.Should().Be("Premium");

        var second = result.Last();
        second.Name.Should().Be("Souvenir Pack");
        second.QuantityAvailable.Should().Be("100");
        second.Membership.Should().Be("Standard");

        _rewardServiceMock.VerifyAll();
    }
    #endregion

}
