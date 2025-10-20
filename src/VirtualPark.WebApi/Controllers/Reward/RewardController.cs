using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Rewards.Models;
using VirtualPark.BusinessLogic.Rewards.Service;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Reward.ModelsIn;
using VirtualPark.WebApi.Controllers.Reward.ModelsOut;
using VirtualPark.WebApi.Filters.Authenticator;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.Reward;

[ApiController]
[AuthenticationFilter]
public sealed class RewardController(IRewardService rewardService) : ControllerBase
{
    private readonly IRewardService _rewardService = rewardService;

    [HttpPost("/rewards")]
    [AuthorizationFilter]
    public CreateRewardResponse CreateReward([FromBody] CreateRewardRequest request)
    {
        RewardArgs args = request.ToArgs();
        Guid rewardId = _rewardService.Create(args);
        return new CreateRewardResponse(rewardId.ToString());
    }

    [HttpGet("/rewards/{id}")]
    [AuthorizationFilter]
    public GetRewardResponse GetRewardById(string id)
    {
        var rewardId = ValidationServices.ValidateAndParseGuid(id);
        var reward = _rewardService.Get(rewardId)!;

        return new GetRewardResponse(
            id: reward.Id.ToString(),
            name: reward.Name,
            description: reward.Description,
            cost: reward.Cost.ToString(),
            quantity: reward.QuantityAvailable.ToString(),
            membership: reward.RequiredMembershipLevel.ToString());
    }
}
