using VirtualPark.BusinessLogic.Visitors.Entity;

namespace VirtualPark.BusinessLogic.Tickets.Entity;

public sealed class Ticket
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime Date { get; set; }
    public EntranceType Type { get; set; }
    public Guid EventId { get; set; }
    public Visitor Visitor { get; set; } = null!;
    public Guid QrId { get; set; } = Guid.NewGuid();
}
