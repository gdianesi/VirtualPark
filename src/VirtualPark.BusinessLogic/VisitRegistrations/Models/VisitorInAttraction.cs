using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Models;

public sealed class VisitorInAttraction
{
    public Guid VisitRegistrationId { get; init; }
    public VisitorProfile Visitor { get; init; } = null!;
    public EntranceType TicketType { get; init; }
}
