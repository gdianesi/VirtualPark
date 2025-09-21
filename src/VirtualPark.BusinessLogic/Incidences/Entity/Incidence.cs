using VirtualPark.BusinessLogic.TypeIncidences.Entity;

namespace VirtualPark.BusinessLogic.Incidences.Entity;

public class Incidence
{
    public Guid Id { get; }
    public TypeIncidence Type { get; init; } = null!;

    public Incidence()
    {
        Id = Guid.NewGuid();
    }
}
