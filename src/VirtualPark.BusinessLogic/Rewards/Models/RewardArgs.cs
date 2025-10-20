using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Rewards.Models;

public sealed class RewardArgs(string name)
{
    public string Name { get; } = ValidationServices.ValidateNullOrEmpty(name);
}
