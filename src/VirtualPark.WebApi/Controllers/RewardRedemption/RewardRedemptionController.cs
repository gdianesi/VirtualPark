using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.RewardRedemptions.Models;
using VirtualPark.BusinessLogic.RewardRedemptions.Service;
using VirtualPark.WebApi.Controllers.RewardRedemption.ModelsIn;
using VirtualPark.WebApi.Controllers.RewardRedemption.ModelsOut;
using VirtualPark.WebApi.Filters.Authenticator;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.RewardRedemption;

[ApiController]
[AuthenticationFilter]
public sealed class RewardRedemptionController(IRewardRedemptionService rewardRedemptionService) : ControllerBase
{
    private readonly IRewardRedemptionService _rewardRedemptionService = rewardRedemptionService;

    [HttpPost("/rewards/redemptions")]
    [AuthorizationFilter]
    public CreateRewardRedemptionResponse RedeemReward([FromBody] CreateRewardRedemptionRequest request)
    {
        RewardRedemptionArgs args = request.ToArgs();
        Guid redemptionId = _rewardRedemptionService.RedeemReward(args);
        return new CreateRewardRedemptionResponse(redemptionId.ToString());
    }
}
