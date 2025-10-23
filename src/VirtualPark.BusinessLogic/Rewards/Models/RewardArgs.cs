using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.Rewards.Models;

public sealed class RewardArgs(string name, string description, string cost, string quantity, string membershipLevel)
{
    public string Name { get; } = ValidationServices.ValidateNullOrEmpty(name);
    public string Description { get; } = ValidationServices.ValidateNullOrEmpty(description);
    public int Cost { get; } = ValidationServices.ValidateAndParseInt(cost);
    public int QuantityAvailable { get; } = ValidationServices.ValidateAndParseInt(quantity);
    public Membership RequiredMembershipLevel { get; } = ValidationServices.ParseMembership(membershipLevel);
}
