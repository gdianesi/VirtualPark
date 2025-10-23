using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.Rewards.Entity;

public sealed class Reward
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Cost { get; set; }
    public int QuantityAvailable { get; set; }
    public Membership RequiredMembershipLevel { get; set; }
}
