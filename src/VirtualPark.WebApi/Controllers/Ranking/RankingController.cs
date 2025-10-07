using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Rankings.Service;
using VirtualPark.WebApi.Controllers.Ranking.ModelsIn;
using VirtualPark.WebApi.Controllers.Ranking.ModelsOut;

namespace VirtualPark.WebApi.Controllers.Ranking;

[ApiController]
public sealed class RankingController(IRankingService rankingService) : ControllerBase
{
    private readonly IRankingService _rankingService = rankingService;

    [HttpGet("/ranking")]
    public GetRankingResponse GetRanking([FromQuery] GetRankingRequest request)
    {
        var args = request.ToArgs();
        var ranking = _rankingService.Get(args);
        return MapToResponse(ranking);
    }

    [HttpGet("/rankings")]
    public List<GetRankingResponse> GetAllRankings()
    {
        var rankings = _rankingService.GetAll();
        return rankings.Select(MapToResponse).ToList();
    }

    private static GetRankingResponse MapToResponse(BusinessLogic.Rankings.Entity.Ranking? ranking)
    {
        return new GetRankingResponse(
            id: ranking.Id.ToString(),
            date: ranking.Date.ToString("yyyy-MM-dd"),
            users: ranking.Entries.Select(u => u.Id.ToString()).ToList(),
            period: ranking.Period.ToString());
    }
}
