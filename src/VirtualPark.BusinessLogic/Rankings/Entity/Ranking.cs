namespace VirtualPark.BusinessLogic.Rankings.Entity;

public sealed class Ranking
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime Date { get; set; }
}
