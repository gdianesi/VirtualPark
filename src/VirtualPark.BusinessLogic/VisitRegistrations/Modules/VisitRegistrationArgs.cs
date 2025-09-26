using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Modules;

public sealed class VisitRegistrationArgs(string date)
{
    public DateOnly Date { get; init; } = ValidationServices.ValidateDate(date);
}
