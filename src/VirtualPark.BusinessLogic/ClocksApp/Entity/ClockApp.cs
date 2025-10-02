namespace VirtualPark.BusinessLogic.ClocksApp.Entity;

public sealed class ClockApp
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public int OffsetMinutes { get; set; } = 0;
}
