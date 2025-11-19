using VirtualPark.BusinessLogic.VisitsScore.Entity;

namespace VirtualPark.WebApi.Controllers.VisitsScore.ModelsOut;

public class GetVisitScoreResponse(VisitScore score)
{
    public string Id { get; } = score.Id.ToString();
    public string Origin { get; } = score.Origin;
    public string OccurredAt { get; } = score.OccurredAt
            .ToUniversalTime()
            .ToString("O");
    public int Points { get; } = score.Points;
    public string? DayStrategyName { get; } = score.DayStrategyName;
    public string VisitRegistrationId { get; } = score.VisitRegistrationId.ToString();
}
