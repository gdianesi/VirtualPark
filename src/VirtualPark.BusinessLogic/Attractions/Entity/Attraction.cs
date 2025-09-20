namespace VirtualPark.BusinessLogic.Attractions.Entity;

public sealed class Attraction
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public AttractionType Type { get; set; }
    public string Name { get; set; } = null!;
    public int MiniumAge { get; set; }
    public int Capacity { get; set; }
}
