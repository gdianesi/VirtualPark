using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Tickets.Entity;

namespace VirtualPark.BusinessLogic.Events.Entity;

public sealed class Event
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
    public int Capacity { get; set; }
    public int Cost { get; set; }
    public List<Attraction> Attractions { get; set; } = [];
    public List<Ticket> Tickets { get; set; } = [];
}
