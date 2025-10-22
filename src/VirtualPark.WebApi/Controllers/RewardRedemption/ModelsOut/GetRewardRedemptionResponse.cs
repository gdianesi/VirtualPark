namespace VirtualPark.WebApi.Controllers.RewardRedemption.ModelsOut;

public sealed class GetRewardRedemptionResponse(string id, string rewardId, string visitorId)
{
    public string? Id { get; set; } = id;
    public string? RewardId { get; set; } = rewardId;

    public string? VisitorId { get; set; } = visitorId;
}
