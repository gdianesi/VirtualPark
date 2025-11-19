namespace VirtualPark.WebApi.Controllers.Reward.ModelsOut;

public class GetRewardResponse(BusinessLogic.Rewards.Entity.Reward reward)
{
    public string Id { get; } = reward.Id.ToString();
    public string Name { get; } = reward.Name;
    public string Description { get; } = reward.Description;
    public string Cost { get; } = reward.Cost.ToString();
    public string QuantityAvailable { get; } = reward.QuantityAvailable.ToString();
    public string Membership { get; } = reward.RequiredMembershipLevel.ToString();
}
