namespace VirtualPark.WebApi.Controllers.Tickets.ModelsOut;

public sealed class GetTicketResponse(string id, string type, string date, string qrId, string? eventId, string visitorId)
{
    public string Id { get; } = id;
    public string Type { get; } = type;
    public string Date { get; } = date;
    public string QrId { get; } = qrId;
    public string? EventId { get; } = eventId;
    public string VisitorId { get; } = visitorId;
}
