using VirtualPark.BusinessLogic.Tickets.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.Tickets.ModelsIn;

public sealed class CreateTicketRequest
{
    public string? EventId { get; init; }
    public string VisitorId { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Date { get; init; } = string.Empty;

    public TicketArgs ToArgs()
    {
        var validatedVisitorId = ValidationServices.ValidateNullOrEmpty(VisitorId);
        var validatedType = ValidationServices.ValidateNullOrEmpty(Type);
        var validatedDate = ValidationServices.ValidateNullOrEmpty(Date);

        var validatedEventId = EventId is null
            ? string.Empty
            : ValidationServices.ValidateNullOrEmpty(EventId);

        return new TicketArgs(validatedDate, validatedType, validatedEventId, validatedVisitorId);
    }
}
