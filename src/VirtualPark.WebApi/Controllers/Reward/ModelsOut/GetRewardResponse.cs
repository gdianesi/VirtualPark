namespace VirtualPark.WebApi.Controllers.Reward.ModelsOut;

public class GetRewardResponse(string id, string name, string description, string cost, string quantity)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string Description { get; } = description;
    public string Cost { get; } = cost;
    public string QuantityAvailable { get; } = quantity;
}
