using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitsScore.Service;
using VirtualPark.WebApi.Controllers.VisitsScore.ModelsOut;
using VirtualPark.WebApi.Filters.Authenticator;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.VisitsScore;

[ApiController]
[AuthenticationFilter]
[Route("visitScores")]
public sealed class VisitScoresController(IVisitScoreService visitScoreService) : ControllerBase
{
    private readonly IVisitScoreService _visitScoreService = visitScoreService;

    [HttpGet("getHistory/{visitorId}")]
    [AuthorizationFilter]
    public List<GetVisitScoreResponse> GetHistoryById(string visitorId)
    {
        var id = ValidationServices.ValidateAndParseGuid(visitorId);

        return _visitScoreService
            .GetScoresByVisitorId(id)
            .Select(s => new GetVisitScoreResponse(s))
            .ToList();
    }
}
