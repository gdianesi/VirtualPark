namespace VirtualPark.WebApi.Test.Controllers.RewardRedemption;

[TestClass]
public class RewardRedemptionControllerTest
{
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
            Date = "2025-10-21",
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

}
