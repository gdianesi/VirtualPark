using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Models;

public sealed class VisitRegistrationArgs(string date, VisitorProfileArgs visitorProfile)
{
    public DateOnly Date { get; init; } = ValidationServices.ValidateDateOnly(date);
    public VisitorProfileArgs VisitorProfile { get; init; } = visitorProfile;
}
