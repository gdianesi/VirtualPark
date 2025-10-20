namespace VirtualPark.WebApi.Controllers.Reward.ModelsOut;

public class GetRewardResponse(string id, string name)
{
    public string Id { get; } = id;
    public object Name { get; } = name;
}
