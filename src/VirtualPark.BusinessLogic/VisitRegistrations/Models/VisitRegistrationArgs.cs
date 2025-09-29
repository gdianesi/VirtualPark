using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Models;

public sealed class VisitRegistrationArgs(string date, VisitorProfileArgs visitorProfile, List<string> attractions)
{
    public DateOnly Date { get; init; } = ValidationServices.ValidateDateOnly(date);
    public VisitorProfileArgs VisitorProfile { get; init; } = visitorProfile;
    public List<Guid> AttractionsId { get; init; } = ValidateAndParseGuidList(attractions);

    private static List<Guid> ValidateAndParseGuidList(List<string> values)
    {
        var result = new List<Guid>();
        foreach(var value in values)
        {
            result.Add(ValidationServices.ValidateAndParseGuid(value));
        }

        return result;
    }
}
