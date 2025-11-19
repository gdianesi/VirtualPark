using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.VisitRegistrations.Models;

public sealed class VisitRegistrationArgs(List<string> attractions, string visitorProfileId, string ticketId)
{
    public Guid VisitorProfileId { get; } = ValidationServices.ValidateAndParseGuid(visitorProfileId);
    public List<Guid> AttractionsId { get; } = ValidateAndParseGuidList(attractions);
    public Guid TicketId { get; } = ValidationServices.ValidateAndParseGuid(ticketId);

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
