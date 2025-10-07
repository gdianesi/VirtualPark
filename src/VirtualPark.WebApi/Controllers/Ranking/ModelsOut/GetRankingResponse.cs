namespace VirtualPark.WebApi.Controllers.Ranking.ModelsOut;

public sealed class GetRankingResponse(string id)
{
    public string Id { get; } = id;
}
