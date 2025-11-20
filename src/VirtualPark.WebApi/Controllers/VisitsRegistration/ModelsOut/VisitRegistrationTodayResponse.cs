using VirtualPark.BusinessLogic.VisitRegistrations.Entity;

namespace VirtualPark.WebApi.Controllers.VisitsRegistration.ModelsOut;

public sealed class VisitRegistrationTodayResponse(VisitRegistration visit)
{
    public Guid VisitRegistrationId { get; } = visit.Id;
}
