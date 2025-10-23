using FluentAssertions;
using VirtualPark.BusinessLogic.RewardRedemptions.Models;
using VirtualPark.WebApi.Controllers.RewardRedemption.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.RewardRedemption.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateRewardRedemptionRequest")]
public sealed class CreateRewardRedemptionRequestTest
{
    #region RewardId

    [TestMethod]
    [TestCategory("Validation")]
    public void RewardId_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var request = new CreateRewardRedemptionRequest { RewardId = id };
        request.RewardId.Should().Be(id);
    }

    #endregion

    #region VisitorId

    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorId_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var request = new CreateRewardRedemptionRequest { VisitorId = id };
        request.VisitorId.Should().Be(id);
    }

    #endregion

    #region Date

    [TestMethod]
    [TestCategory("Validation")]
    public void Date_Getter_ReturnsAssignedValue()
    {
        var request = new CreateRewardRedemptionRequest { Date = "2025-12-21" };
        request.Date.Should().Be("2025-12-21");
    }

    #endregion

    #region Points spend

    [TestMethod]
    [TestCategory("Validation")]
    public void PointsSpent_Getter_ReturnsAssignedValue()
    {
        var request = new CreateRewardRedemptionRequest { PointsSpent = "1500" };
        request.PointsSpent.Should().Be("1500");
    }

    #endregion

    #region ToArgs

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldMapAllFields_WhenAllAreValid()
    {
        var rewardId = Guid.NewGuid().ToString();
        var visitorId = Guid.NewGuid().ToString();

        var request = new CreateRewardRedemptionRequest
        {
            RewardId = rewardId,
            VisitorId = visitorId,
            Date = "2025-12-21",
            PointsSpent = "1200"
        };

        RewardRedemptionArgs args = request.ToArgs();

        args.Should().NotBeNull();
        args.RewardId.Should().Be(Guid.Parse(rewardId));
        args.VisitorId.Should().Be(Guid.Parse(visitorId));
        args.Date.Should().Be(new DateOnly(2025, 12, 21));
        args.PointsSpent.Should().Be(1200);
    }

    #endregion
}
