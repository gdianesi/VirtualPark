namespace VirtualPark.BusinessLogic.Rewards.Models;

public sealed class RewardArgs(string name)
{
    public string Name { get; init; } = name;
}
