namespace VirtualPark.BusinessLogic.Strategy.Entity;

public sealed class ActiveStrategy
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string StrategyKey { get; init; } = null!;
    public DateTime Date { get; set; } = DateTime.Today;
}
