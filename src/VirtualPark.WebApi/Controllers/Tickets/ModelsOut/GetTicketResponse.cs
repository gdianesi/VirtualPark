using VirtualPark.BusinessLogic.Tickets.Entity;

namespace VirtualPark.WebApi.Controllers.Tickets.ModelsOut;

public sealed class GetTicketResponse
{
    public string Id { get; }
    public string Type { get; }
    public string Date { get; }
    public string QrId { get; }
    public string? EventId { get; }
    public string VisitorId { get; }

    public GetTicketResponse(Ticket ticket)
    {
        Id = ticket.Id.ToString();
        Type = ticket.Type.ToString();
        Date = ticket.Date.ToString("yyyy-MM-dd");
        QrId = ticket.QrId.ToString();
        EventId = ticket.EventId?.ToString();
        VisitorId = ticket.VisitorProfileId.ToString();
    }
}
