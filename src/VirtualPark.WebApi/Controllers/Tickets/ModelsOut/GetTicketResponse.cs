using VirtualPark.BusinessLogic.Tickets.Entity;

namespace VirtualPark.WebApi.Controllers.Tickets.ModelsOut;

public sealed class GetTicketResponse(Ticket ticket)
{
    public string Id { get; } = ticket.Id.ToString();
    public string Type { get; } = ticket.Type.ToString();
    public string Date { get; } = ticket.Date.ToString("yyyy-MM-dd");
    public string QrId { get; } = ticket.QrId.ToString();
    public string? EventId { get; } = ticket.EventId?.ToString();
    public string VisitorId { get; } = ticket.VisitorProfileId.ToString();
}
