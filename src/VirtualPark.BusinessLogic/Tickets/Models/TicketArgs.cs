namespace VirtualPark.BusinessLogic.Tickets.Models;

public sealed class TicketArgs(string date, EntranceType type, Guid eventId, Guid visitorId)
{
    public DateOnly Date { get; } =
        DateOnly.TryParseExact(date, "yyyy-MM-dd", out var parsedDate)
            ? parsedDate
            : default;

    public EntranceType Type { get; } = type;
    public Guid EventId { get; } = eventId;
    public Guid VisitorId { get; } = visitorId;
}
