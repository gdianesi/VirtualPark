using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Entity;

public sealed class VisitRegistration
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime Date { get; set; } = DateTime.Today;
    public List<Attraction> Attractions { get; set; } = [];
    public VisitorProfile Visitor { get; set; } = null!;
    public Guid VisitorId { get; set; }
    public Ticket Ticket { get; set; } = null!;
    public Guid TicketId { get; set; }
    public bool IsActive { get; set; }
}
