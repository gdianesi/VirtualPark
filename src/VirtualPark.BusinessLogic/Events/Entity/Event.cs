namespace VirtualPark.BusinessLogic.Events.Entity;

public sealed class Event
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
}
