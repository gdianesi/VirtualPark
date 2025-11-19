namespace VirtualPark.WebApi.Controllers.RewardRedemption.ModelsOut;

public sealed class GetRewardRedemptionResponse
{
    public string Id { get; }
    public string RewardId { get; }
    public string VisitorId { get; }
    public string Date { get; }
    public string PointsSpent { get; }

    public GetRewardRedemptionResponse(BusinessLogic.RewardRedemptions.Entity.RewardRedemption redemption)
    {
        Id = redemption.Id.ToString();
        RewardId = redemption.RewardId.ToString();
        VisitorId = redemption.VisitorId.ToString();
        Date = redemption.Date.ToString("yyyy-MM-dd");
        PointsSpent = redemption.PointsSpent.ToString();
    }
}
