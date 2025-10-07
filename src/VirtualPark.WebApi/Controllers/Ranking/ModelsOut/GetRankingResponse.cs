namespace VirtualPark.WebApi.Controllers.Ranking.ModelsOut;

public sealed class GetRankingResponse(string id, string date, List<string> users)
{
    public string Id { get; } = id;
    public string Date { get; } = date;
    public List<string> Users { get; } = users;
}
