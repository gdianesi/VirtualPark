using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Tickets.Models;

public sealed class TicketArgs(string date, EntranceType type, Guid eventId, Guid visitorId)
{
    public DateOnly Date { get; } = ValidationServices.ValidateDateOnly(date);

    public EntranceType Type { get; } = type;
    public Guid EventId { get; } = eventId;
    public Guid VisitorId { get; } = visitorId;
}
