using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.RewardRedemptions.Models;

public sealed class RewardRedemptionArgs(string rewardId, string visitorId, string date, string pointsSpent)
{
    public Guid RewardId { get; } = ValidationServices.ValidateAndParseGuid(rewardId);
    public Guid VisitorId { get; } = ValidationServices.ValidateAndParseGuid(visitorId);
    public DateOnly Date { get; } = ValidationServices.ValidateDateOnly(date);
    public int PointsSpent { get; } = ValidationServices.ValidateAndParseInt(pointsSpent);
}
