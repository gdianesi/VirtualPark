using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Visitors.Entity;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Entity;

public sealed class VisitRegistration
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime Date { get; set; } = DateTime.Today;
    public List<Attraction> Attractions { get; set; } = [];
    public Visitor Visitor { get; set; } = null!;
}
