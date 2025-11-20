using VirtualPark.BusinessLogic.RewardRedemptions.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.RewardRedemption.ModelsIn;

public sealed class CreateRewardRedemptionRequest
{
    public string? RewardId { get; init; }
    public string? VisitorId { get; init; }
    public string? Date { get; set; }
    public string? PointsSpent { get; set; }

    public RewardRedemptionArgs ToArgs()
    {
        return new RewardRedemptionArgs(
            ValidationServices.ValidateNullOrEmpty(RewardId),
            ValidationServices.ValidateNullOrEmpty(VisitorId),
            ValidationServices.ValidateNullOrEmpty(Date),
            ValidationServices.ValidateNullOrEmpty(PointsSpent));
    }
}
