namespace VirtualPark.WebApi.Controllers.RewardRedemption.ModelsOut;

public sealed class GetRewardRedemptionResponse(string id)
{
    public string? Id { get; set; } = id;
}
