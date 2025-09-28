namespace VirtualPark.BusinessLogic.TypeIncidences.Entity;

public class TypeIncidence
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Type { get; set; } = null!;
}
