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
[Route("rewards")]
public sealed class RewardController(IRewardService rewardService) : ControllerBase
{
    private readonly IRewardService _rewardService = rewardService;

    [HttpPost]
    [AuthorizationFilter]
    public CreateRewardResponse CreateReward([FromBody] CreateRewardRequest request)
    {
        RewardArgs args = request.ToArgs();
        Guid rewardId = _rewardService.Create(args);
        return new CreateRewardResponse(rewardId.ToString());
    }

    [HttpGet("{id}")]
    [AuthorizationFilter]
    public GetRewardResponse GetRewardById(string id)
    {
        var rewardId = ValidationServices.ValidateAndParseGuid(id);
        var reward = _rewardService.Get(rewardId)!;

        return MapGetByIdRewardResponse(reward);
    }

    private static GetRewardResponse MapGetByIdRewardResponse(BusinessLogic.Rewards.Entity.Reward reward)
    {
        return new GetRewardResponse(
            id: reward.Id.ToString(),
            name: reward.Name,
            description: reward.Description,
            cost: reward.Cost.ToString(),
            quantity: reward.QuantityAvailable.ToString(),
            membership: reward.RequiredMembershipLevel.ToString());
    }

    [HttpGet]
    [AuthorizationFilter]
    public List<GetRewardResponse> GetAllRewards()
    {
        return MapToGetAllRewardResponses();
    }

    private List<GetRewardResponse> MapToGetAllRewardResponses()
    {
        return _rewardService
            .GetAll()
            .Select(r => new GetRewardResponse(
                id: r.Id.ToString(),
                name: r.Name,
                description: r.Description,
                cost: r.Cost.ToString(),
                quantity: r.QuantityAvailable.ToString(),
                membership: r.RequiredMembershipLevel.ToString()))
            .ToList();
    }

    [HttpDelete("{id}")]
    [AuthorizationFilter]
    public void DeleteReward(string id)
    {
        var rewardId = ValidationServices.ValidateAndParseGuid(id);
        _rewardService.Remove(rewardId);
    }

    [HttpPut("{id}")]
    [AuthorizationFilter]
    public void UpdateReward(CreateRewardRequest request, string id)
    {
        var rewardId = ValidationServices.ValidateAndParseGuid(id);
        var args = request.ToArgs();
        _rewardService.Update(args, rewardId);
    }
}
