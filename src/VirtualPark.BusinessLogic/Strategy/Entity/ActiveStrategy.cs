namespace VirtualPark.BusinessLogic.Strategy.Entity;

public sealed class ActiveStrategy
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string StrategyKey { get; set; } = null!;
    public DateOnly Date { get; set; }
}
