namespace VirtualPark.WebApi.Controllers.RewardRedemption.ModelsOut;

public sealed class GetRewardRedemptionResponse(string id, string rewardId, string visitorId, string date, string pointsSpend)
{
    public string? Id { get; } = id;
    public string? RewardId { get; } = rewardId;

    public string? VisitorId { get; } = visitorId;
    public string? Date { get; } = date;
    public string? PointsSpent { get; } = pointsSpend;
}
