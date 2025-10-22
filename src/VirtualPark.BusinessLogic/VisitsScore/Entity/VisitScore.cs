namespace VirtualPark.BusinessLogic.VisitsScore.Entity;

public class VisitScore
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Origin { get; set; } = null!;
}
