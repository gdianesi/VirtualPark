namespace VirtualPark.WebApi.Controllers.VisitsScore.ModelsOut;

public class GetVisitScoreResponse(string id, string origin, string occurredAt, int points, string? dayStrategyName,
    string visitRegistrationId)
{
    public string Id { get; } = id;
    public string Origin { get; } = origin;
    public string OccurredAt { get; } = occurredAt;
    public int Points { get; } = points;
    public string? DayStrategyName { get; } = dayStrategyName;
    public string VisitRegistrationId { get; } = visitRegistrationId;
}
