using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Rankings.Service;
using VirtualPark.WebApi.Controllers.Ranking.ModelsIn;
using VirtualPark.WebApi.Controllers.Ranking.ModelsOut;

namespace VirtualPark.WebApi.Controllers.Ranking;

[ApiController]
[Route("rankings")]
public sealed class RankingController(IRankingService rankingService) : ControllerBase
{
    private readonly IRankingService _rankingService = rankingService;

    [HttpGet("filter")]
    public GetRankingResponse GetRanking([FromQuery] GetRankingRequest request)
    {
        var args = request.ToArgs();
        var ranking = _rankingService.Get(args);
        return new GetRankingResponse(ranking);
    }

    [HttpGet]
    public List<GetRankingResponse> GetAllRankings()
    {
        return _rankingService.GetAll()
            .Select(r => new GetRankingResponse(r))
            .ToList();
    }
}
