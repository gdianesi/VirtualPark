namespace VirtualPark.WebApi.Controllers.Tickets.ModelsOut;

public sealed class CreateTicketResponse(string id)
{
    public string Id { get; } = id;
}
