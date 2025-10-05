using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;

namespace VirtualPark.BusinessLogic.Incidences.Entity;

public sealed class Incidence
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public TypeIncidence Type { get; set; } = null!;
    public Guid TypeIncidenceId { get; set; }
    public string Description { get; set; } = null!;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public Attraction Attraction { get; set; } = null!;
    public Guid AttractionId { get; set; }
    public bool Active { get; set; }
}
