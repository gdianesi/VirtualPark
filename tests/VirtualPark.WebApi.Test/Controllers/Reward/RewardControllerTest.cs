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
}
