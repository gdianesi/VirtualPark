using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Tickets.Models;

public sealed class TicketArgs(string date, string type, string eventId, string visitorId)
{
    public DateOnly Date { get; } = ValidationServices.ValidateDateOnly(date);

    public EntranceType Type { get; } = ValidationServices.ParseEntranceType(type);

    public Guid EventId { get; } = ValidationServices.ValidateAndParseGuid(eventId);
    public Guid VisitorId { get; } = ValidateAndParseGuid(visitorId);

    private static Guid ValidateAndParseGuid(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or empty.");
        }

        Guid result = Guid.Parse(value);
        return result;
    }
}
