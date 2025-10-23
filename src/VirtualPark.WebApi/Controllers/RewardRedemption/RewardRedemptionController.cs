using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.RewardRedemptions.Models;
using VirtualPark.BusinessLogic.RewardRedemptions.Service;
using VirtualPark.BusinessLogic.Validations.Services;
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

    [HttpGet("/rewards/redemptions/{id}")]
    [AuthorizationFilter]
    public GetRewardRedemptionResponse GetRewardRedemptionById(string id)
    {
        var redemptionId = ValidationServices.ValidateAndParseGuid(id);
        var redemption = _rewardRedemptionService.Get(redemptionId)!;

        return new GetRewardRedemptionResponse(
            id: redemption.Id.ToString(),
            rewardId: redemption.RewardId.ToString(),
            visitorId: redemption.VisitorId.ToString(),
            date: redemption.Date.ToString("yyyy-MM-dd"),
            pointsSpend: redemption.PointsSpent.ToString());
    }

    [HttpGet("/rewards/redemptions")]
    [AuthorizationFilter]
    public List<GetRewardRedemptionResponse> GetAllRewardRedemptions()
    {
        return _rewardRedemptionService
            .GetAll()
            .Select(r => new GetRewardRedemptionResponse(
                id: r.Id.ToString(),
                rewardId: r.RewardId.ToString(),
                visitorId: r.VisitorId.ToString(),
                date: r.Date.ToString("yyyy-MM-dd"),
                pointsSpend: r.PointsSpent.ToString()))
            .ToList();
    }

    [HttpGet("/rewards/redemptions/visitor/{visitorId}")]
    [AuthorizationFilter]
    public List<GetRewardRedemptionResponse> GetRewardRedemptionsByVisitor(string visitorId)
    {
        var parsedId = ValidationServices.ValidateAndParseGuid(visitorId);
        return _rewardRedemptionService
            .GetByVisitor(parsedId)
            .Select(r => new GetRewardRedemptionResponse(
                id: r.Id.ToString(),
                rewardId: r.RewardId.ToString(),
                visitorId: r.VisitorId.ToString(),
                date: r.Date.ToString("yyyy-MM-dd"),
                pointsSpend: r.PointsSpent.ToString()))
            .ToList();
    }
}
