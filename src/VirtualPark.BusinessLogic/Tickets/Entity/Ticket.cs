using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.Tickets.Entity;

public sealed class Ticket
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime Date { get; set; }
    public EntranceType Type { get; set; }
    public Guid EventId { get; set; }
    public VisitorProfile Visitor { get; set; } = null!;
    public Guid QrId { get; set; } = Guid.NewGuid();
}
