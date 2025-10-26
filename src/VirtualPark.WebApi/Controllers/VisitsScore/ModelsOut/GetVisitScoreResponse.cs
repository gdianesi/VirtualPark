namespace VirtualPark.WebApi.Controllers.VisitsScore.ModelsOut;

public class GetVisitScoreResponse(string id, string origin,  string occurredAt, int points)
{
    public string Id { get; } = id;
    public string Origin { get; } = origin;
    public string OccurredAt { get; } = occurredAt;
    public int Points { get; } = points;
}
