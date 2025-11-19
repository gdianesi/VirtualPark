namespace VirtualPark.WebApi.Controllers.Ranking.ModelsOut;

public sealed class GetRankingResponse(BusinessLogic.Rankings.Entity.Ranking? ranking)
{
    public string Id { get; } = ranking.Id.ToString();
    public string Date { get; } = ranking.Date.ToString("yyyy-MM-dd");
    public List<string> Users { get; } = ranking.Entries
            .Select(e => e.Id.ToString())
            .ToList();
    public List<string> Scores { get; } = ranking.Entries
            .Select(e => (e.VisitorProfile?.Score ?? 0).ToString())
            .ToList();
    public string Period { get; } = ranking.Period.ToString();
}
