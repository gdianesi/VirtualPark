namespace VirtualPark.WebApi.Controllers.Tickets.ModelsOut;

public sealed class GetTicketResponse(string id, string type, string date)
{
    public string Id { get; } = id;
    public string Type { get; } = type;
    public string Date { get; set; } = date;

}
