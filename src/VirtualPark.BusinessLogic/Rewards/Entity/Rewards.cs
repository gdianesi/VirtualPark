namespace VirtualPark.BusinessLogic.Rewards.Entity;

public sealed class Reward
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Cost { get; set; }
}
