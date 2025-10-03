using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Tickets.Models;

public sealed class TicketArgs(string date, string type, string eventId, string visitorId)
{
    public DateTime Date { get; } = ValidationServices.ValidateDateTimeTickets(date);

    public EntranceType Type { get; } = ValidationServices.ParseEntranceType(type);

    public Guid EventId { get; } = ValidationServices.ValidateAndParseGuid(eventId);
    public Guid VisitorId { get; } = ValidationServices.ValidateAndParseGuid(visitorId);
}
