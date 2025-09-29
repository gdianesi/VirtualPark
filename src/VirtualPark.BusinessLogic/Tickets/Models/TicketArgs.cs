using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Tickets.Models;

public sealed class TicketArgs(string date, string type, Guid eventId, Guid visitorId)
{
    public DateOnly Date { get; } = ValidationServices.ValidateDateOnly(date);

    public EntranceType Type { get; } = GenerateEntranceType(type);

    private static EntranceType GenerateEntranceType(string type)
    {
        var isNotValid = !Enum.TryParse<EntranceType>(type, true, out var parsedType);
        if (isNotValid)
        {
            throw new ArgumentException(
                $"Invalid entrance type value: {type}");
        }

        return parsedType;
    }

    public Guid EventId { get; } = eventId;
    public Guid VisitorId { get; } = visitorId;
}
