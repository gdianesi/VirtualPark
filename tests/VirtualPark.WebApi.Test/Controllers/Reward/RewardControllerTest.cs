using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Rewards.Models;
using VirtualPark.BusinessLogic.Rewards.Service;
using VirtualPark.WebApi.Controllers.Reward;
using VirtualPark.WebApi.Controllers.Reward.ModelsIn;
using VirtualPark.WebApi.Controllers.Reward.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Reward;

[TestClass]
public sealed class RewardControllerTest
{
    private Mock<IRewardService> _rewardServiceMock = null!;
    private RewardController _rewardController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _rewardServiceMock = new Mock<IRewardService>(MockBehavior.Strict);
        _rewardController = new RewardController(_rewardServiceMock.Object);
    }

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

        var expectedArgs = request.ToArgs();

        _rewardServiceMock
            .Setup(s => s.Create(It.Is<RewardArgs>(a =>
                a.Name == expectedArgs.Name &&
                a.Description == expectedArgs.Description &&
                a.Cost == expectedArgs.Cost &&
                a.QuantityAvailable == expectedArgs.QuantityAvailable &&
                a.RequiredMembershipLevel == expectedArgs.RequiredMembershipLevel)))
            .Returns(returnId);

        var response = _rewardController.CreateReward(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<CreateRewardResponse>();
        response.Id.Should().Be(returnId.ToString());

        _rewardServiceMock.VerifyAll();
    }
}
