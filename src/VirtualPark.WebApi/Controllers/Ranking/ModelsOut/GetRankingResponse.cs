namespace VirtualPark.WebApi.Controllers.Ranking.ModelsOut;

public sealed class GetRankingResponse(string id, string date)
{
    public string Id { get; } = id;
    public string Date { get; } = date;
}
