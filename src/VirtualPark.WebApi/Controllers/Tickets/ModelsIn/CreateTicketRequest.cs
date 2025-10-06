namespace VirtualPark.WebApi.Controllers.Tickets.ModelsIn;

public class CreateTicketRequest(string visitorId)
{
    public string VisitorId { get; set; } = visitorId;
}
