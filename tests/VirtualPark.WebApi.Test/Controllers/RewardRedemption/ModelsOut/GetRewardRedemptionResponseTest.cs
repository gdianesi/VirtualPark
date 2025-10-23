using FluentAssertions;
using VirtualPark.WebApi.Controllers.RewardRedemption.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.RewardRedemption.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetRewardRedemptionResponse")]
public sealed class GetRewardRedemptionResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();

        var response = new GetRewardRedemptionResponse(
            id, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            "2025-12-21",
            "1200");

        response.Id.Should().Be(id);
    }
    #endregion

    #region RewardId
    [TestMethod]
    [TestCategory("Validation")]
    public void RewardId_Getter_ReturnsAssignedValue()
    {
        var rewardId = Guid.NewGuid().ToString();

        var response = new GetRewardRedemptionResponse(
            Guid.NewGuid().ToString(),
            rewardId, Guid.NewGuid().ToString(),
            "2025-12-21",
            "1200");

        response.RewardId.Should().Be(rewardId);
    }
    #endregion

    #region VisitorId
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorId_Getter_ReturnsAssignedValue()
    {
        var visitorId = Guid.NewGuid().ToString();

        var response = new GetRewardRedemptionResponse(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            visitorId,
            "2025-12-21",
            "1200");

        response.VisitorId.Should().Be(visitorId);
    }
    #endregion

    #region Date
    [TestMethod]
    [TestCategory("Validation")]
    public void Date_Getter_ReturnsAssignedValue()
    {
        var response = new GetRewardRedemptionResponse(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            "2025-12-21",
            "1200");

        response.Date.Should().Be("2025-12-21");
    }
    #endregion

    #region PointsSpend
    [TestMethod]
    [TestCategory("Validation")]
    public void PointsSpent_Getter_ReturnsAssignedValue()
    {
        var response = new GetRewardRedemptionResponse(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            "2025-10-21",
            "1200");

        response.PointsSpent.Should().Be("1200");
    }
    #endregion
}
