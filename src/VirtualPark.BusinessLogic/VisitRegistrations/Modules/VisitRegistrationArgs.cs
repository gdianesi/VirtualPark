namespace VirtualPark.BusinessLogic.VisitRegistrations.Modules;

public sealed class VisitRegistrationArgs(string date)
{
    public string Date { get; init; } = date;
}
