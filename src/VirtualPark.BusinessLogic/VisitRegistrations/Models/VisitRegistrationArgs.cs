using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Models;

public sealed class VisitRegistrationArgs(List<string> attractions, string visitorProfileId, string ticketId)
{
    public Guid VisitorProfileId { get; init; } = ValidationServices.ValidateAndParseGuid(visitorProfileId);
    public List<Guid> AttractionsId { get; init; } = ValidateAndParseGuidList(attractions);
    public Guid TicketId { get; init; } = Guid.Parse(ticketId);

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
