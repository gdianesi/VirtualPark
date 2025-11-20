namespace VirtualPark.WebApi.Controllers.Reward.ModelsOut;

public sealed class CreateRewardResponse(string id)
{
    public string Id { get; } = id;
}
