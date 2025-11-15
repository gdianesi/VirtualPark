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
        return MapToResponse(ranking);
    }

    [HttpGet]
    public List<GetRankingResponse> GetAllRankings()
    {
        var rankings = _rankingService.GetAll();
        return rankings.Select(MapToResponse).ToList();
    }

    private static GetRankingResponse MapToResponse(BusinessLogic.Rankings.Entity.Ranking? ranking)
    {
        var users = ranking.Entries
            .Select(u => u.Id.ToString())
            .ToList();

        var scoresList = ranking.Entries
            .Select(u => (u.VisitorProfile?.Score ?? 0).ToString())
            .ToList();

        return new GetRankingResponse(
            id: ranking.Id.ToString(),
            date: ranking.Date.ToString("yyyy-MM-dd"),
            users: users,
            scores: scoresList,
            period: ranking.Period.ToString());
    }
}
