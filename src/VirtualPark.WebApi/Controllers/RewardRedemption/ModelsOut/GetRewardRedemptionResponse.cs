namespace VirtualPark.WebApi.Controllers.RewardRedemption.ModelsOut;

public sealed class GetRewardRedemptionResponse(string id, string rewardId, string visitorId, string date)
{
    public string? Id { get; } = id;
    public string? RewardId { get; } = rewardId;

    public string? VisitorId { get; } = visitorId;
    public string? Date { get; set; } = date;
}
