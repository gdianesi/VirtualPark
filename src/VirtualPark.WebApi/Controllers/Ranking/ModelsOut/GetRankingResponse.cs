namespace VirtualPark.WebApi.Controllers.Ranking.ModelsOut;

public sealed class GetRankingResponse(string id, string date, List<string> users, List<string> scores, string period)
{
    public string Id { get; } = id;
    public string Date { get; } = date;
    public List<string> Users { get; } = users;
    public List<string> Scores { get; } = scores;
    public string Period { get; } = period;
}
