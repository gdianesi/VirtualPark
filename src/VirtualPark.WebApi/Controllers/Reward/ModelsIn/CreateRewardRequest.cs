using VirtualPark.BusinessLogic.Rewards.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.Reward.ModelsIn;

public sealed class CreateRewardRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public string? Cost { get; set; }
    public string? QuantityAvailable { get; set; }
    public string? Membership { get; set; }

    public RewardArgs ToArgs()
    {
        return new RewardArgs(
            ValidationServices.ValidateNullOrEmpty(Name),
            ValidationServices.ValidateNullOrEmpty(Description),
            ValidationServices.ValidateNullOrEmpty(Cost),
            ValidationServices.ValidateNullOrEmpty(QuantityAvailable),
            ValidationServices.ValidateNullOrEmpty(Membership));
    }
}
