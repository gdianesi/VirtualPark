namespace VirtualPark.WebApi.Controllers.Events.ModelsOut;

public sealed class GetEventResponse(string id, string name, string date)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string Date { get; set; } = date;
}
