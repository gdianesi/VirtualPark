namespace VirtualPark.WebApi.Controllers.RewardRedemption.ModelsOut;

public sealed class GetRewardRedemptionResponse(BusinessLogic.RewardRedemptions.Entity.RewardRedemption redemption)
{
    public string Id { get; } = redemption.Id.ToString();
    public string RewardId { get; } = redemption.RewardId.ToString();
    public string VisitorId { get; } = redemption.VisitorId.ToString();
    public string Date { get; } = redemption.Date.ToString("yyyy-MM-dd");
    public string PointsSpent { get; } = redemption.PointsSpent.ToString();
}
