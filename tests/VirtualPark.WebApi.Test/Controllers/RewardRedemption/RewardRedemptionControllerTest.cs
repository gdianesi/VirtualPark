using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.RewardRedemptions.Models;
using VirtualPark.BusinessLogic.RewardRedemptions.Service;
using VirtualPark.WebApi.Controllers.RewardRedemption;
using VirtualPark.WebApi.Controllers.RewardRedemption.ModelsIn;
using VirtualPark.WebApi.Controllers.RewardRedemption.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.RewardRedemption;

[TestClass]
public class RewardRedemptionControllerTest
{
    private Mock<IRewardRedemptionService> _rewardRedemptionServiceMock = null!;
    private RewardRedemptionController _rewardRedemptionController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _rewardRedemptionServiceMock = new Mock<IRewardRedemptionService>(MockBehavior.Strict);
        _rewardRedemptionController = new RewardRedemptionController(_rewardRedemptionServiceMock.Object);
    }

    #region RedeemReward
    [TestMethod]
    public void RedeemReward_ValidInput_ShouldReturnCreatedRewardRedemptionResponse()
    {
        var rewardId = Guid.NewGuid().ToString();
        var visitorId = Guid.NewGuid().ToString();
        var returnId = Guid.NewGuid();

        var request = new CreateRewardRedemptionRequest
        {
            RewardId = rewardId,
            VisitorId = visitorId,
            Date = "2025-12-21",
            PointsSpent = "1200"
        };

        var expectedArgs = request.ToArgs();

        _rewardRedemptionServiceMock
            .Setup(s => s.RedeemReward(It.Is<RewardRedemptionArgs>(a =>
                a.RewardId == expectedArgs.RewardId &&
                a.VisitorId == expectedArgs.VisitorId &&
                a.Date == expectedArgs.Date &&
                a.PointsSpent == expectedArgs.PointsSpent)))
            .Returns(returnId);

        var response = _rewardRedemptionController.RedeemReward(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<CreateRewardRedemptionResponse>();
        response.Id.Should().Be(returnId.ToString());

        _rewardRedemptionServiceMock.VerifyAll();
    }
    #endregion

    #region Get
    [TestMethod]
    public void GetRewardRedemptionById_ValidInput_ShouldReturnGetRewardRedemptionResponse()
    {
        var redemption = new BusinessLogic.RewardRedemptions.Entity.RewardRedemption
        {
            RewardId = Guid.NewGuid(),
            VisitorId = Guid.NewGuid(),
            Date = new DateOnly(2025, 12, 21),
            PointsSpent = 1200
        };

        var id = redemption.Id;

        _rewardRedemptionServiceMock
            .Setup(s => s.Get(id))
            .Returns(redemption);

        var response = _rewardRedemptionController.GetRewardRedemptionById(id.ToString());

        response.Should().NotBeNull();
        response.Should().BeOfType<GetRewardRedemptionResponse>();
        response.Id.Should().Be(id.ToString());
        response.RewardId.Should().Be(redemption.RewardId.ToString());
        response.VisitorId.Should().Be(redemption.VisitorId.ToString());
        response.Date.Should().Be("2025-12-21");
        response.PointsSpent.Should().Be("1200");

        _rewardRedemptionServiceMock.VerifyAll();
    }
    #endregion

    #region GetAll
    [TestMethod]
    public void GetAllRewardRedemptions_ShouldReturnMappedList()
    {
        var redemption1 = new BusinessLogic.RewardRedemptions.Entity.RewardRedemption
        {
            RewardId = Guid.NewGuid(),
            VisitorId = Guid.NewGuid(),
            Date = new DateOnly(2025, 12, 21),
            PointsSpent = 1000
        };

        var redemption2 = new BusinessLogic.RewardRedemptions.Entity.RewardRedemption
        {
            RewardId = Guid.NewGuid(),
            VisitorId = Guid.NewGuid(),
            Date = new DateOnly(2025, 12, 22),
            PointsSpent = 500
        };

        var list = new List<BusinessLogic.RewardRedemptions.Entity.RewardRedemption> { redemption1, redemption2 };

        _rewardRedemptionServiceMock
            .Setup(s => s.GetAll())
            .Returns(list);

        var result = _rewardRedemptionController.GetAllRewardRedemptions();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var first = result.First();
        first.PointsSpent.Should().Be("1000");
        first.Date.Should().Be("2025-12-21");

        var second = result.Last();
        second.PointsSpent.Should().Be("500");
        second.Date.Should().Be("2025-12-22");

        _rewardRedemptionServiceMock.VerifyAll();
    }
    #endregion

    #region GetByVisitor
    [TestMethod]
    public void GetRewardRedemptionsByVisitor_ShouldReturnFilteredList()
    {
        var visitorId = Guid.NewGuid();

        var redemption = new BusinessLogic.RewardRedemptions.Entity.RewardRedemption
        {
            RewardId = Guid.NewGuid(),
            VisitorId = visitorId,
            Date = new DateOnly(2025, 12, 21),
            PointsSpent = 800
        };

        _rewardRedemptionServiceMock
            .Setup(s => s.GetByVisitor(visitorId))
            .Returns([redemption]);

        var result = _rewardRedemptionController.GetRewardRedemptionsByVisitor(visitorId.ToString());

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().VisitorId.Should().Be(visitorId.ToString());
        result.First().PointsSpent.Should().Be("800");

        _rewardRedemptionServiceMock.VerifyAll();
    }
    #endregion
}
