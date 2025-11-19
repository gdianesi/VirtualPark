using FluentAssertions;
using VirtualPark.WebApi.Controllers.RewardRedemption.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.RewardRedemption.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetRewardRedemptionResponse")]
public sealed class GetRewardRedemptionResponseTest
{
    private static BusinessLogic.RewardRedemptions.Entity.RewardRedemption BuildEntity(
        Guid? id = null,
        Guid? rewardId = null,
        Guid? visitorId = null,
        DateOnly? date = null,
        int? pointsSpent = null)
    {
        return new BusinessLogic.RewardRedemptions.Entity.RewardRedemption
        {
            Id = id ?? Guid.NewGuid(),
            RewardId = rewardId ?? Guid.NewGuid(),
            VisitorId = visitorId ?? Guid.NewGuid(),
            Date = date ?? new DateOnly(2025, 12, 21),
            PointsSpent = pointsSpent ?? 1200
        };
    }

    #region Id
    [TestMethod]
    public void Id_ShouldMapCorrectly()
    {
        var id = Guid.NewGuid();
        var entity = BuildEntity(id: id);

        var dto = new GetRewardRedemptionResponse(entity);

        dto.Id.Should().Be(id.ToString());
    }
    #endregion

    #region RewardId
    [TestMethod]
    public void RewardId_ShouldMapCorrectly()
    {
        var rid = Guid.NewGuid();
        var entity = BuildEntity(rewardId: rid);

        var dto = new GetRewardRedemptionResponse(entity);

        dto.RewardId.Should().Be(rid.ToString());
    }
    #endregion

    #region VisitorId
    [TestMethod]
    public void VisitorId_ShouldMapCorrectly()
    {
        var vid = Guid.NewGuid();
        var entity = BuildEntity(visitorId: vid);

        var dto = new GetRewardRedemptionResponse(entity);

        dto.VisitorId.Should().Be(vid.ToString());
    }
    #endregion

    #region Date
    [TestMethod]
    public void Date_ShouldMapCorrectly()
    {
        var date = new DateOnly(2026, 01, 15);
        var entity = BuildEntity(date: date);

        var dto = new GetRewardRedemptionResponse(entity);

        dto.Date.Should().Be("2026-01-15");
    }
    #endregion

    #region PointsSpent
    [TestMethod]
    public void PointsSpent_ShouldMapCorrectly()
    {
        var entity = BuildEntity(pointsSpent: 500);

        var dto = new GetRewardRedemptionResponse(entity);

        dto.PointsSpent.Should().Be("500");
    }
    #endregion
}
