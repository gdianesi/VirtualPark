using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitsScore.Entity;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Entity;

public sealed class VisitRegistration
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime Date { get; set; }
    public List<Attraction> Attractions { get; set; } = [];
    public VisitorProfile Visitor { get; set; } = null!;
    public Guid VisitorId { get; set; }
    public Ticket Ticket { get; set; } = null!;
    public Guid TicketId { get; set; }
    public bool IsActive { get; set; }
    public Attraction CurrentAttraction { get; set; } = null!;
    public Guid CurrentAttractionId { get; set; }
    public int DailyScore { get; set; } = 0;
    public List<VisitScore> ScoreEvents { get; set; } = [];
}
