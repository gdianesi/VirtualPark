namespace VirtualPark.WebApi.Controllers.RewardRedemption.ModelsOut;

public class CreateRewardRedemptionResponse(string id)
{
    public string? Id { get; init; } = id;
}
