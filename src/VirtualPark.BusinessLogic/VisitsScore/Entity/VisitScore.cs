namespace VirtualPark.BusinessLogic.VisitsScore.Entity;

public class VisitScore
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Origin { get; set; } = null!;
    public DateTime OccurredAt { get; set; }
    public int Points { get; set; }
    public string? DayStrategyName { get; set; }
}
