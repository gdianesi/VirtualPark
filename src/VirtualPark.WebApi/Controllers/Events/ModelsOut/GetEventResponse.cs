namespace VirtualPark.WebApi.Controllers.Events.ModelsOut;

public sealed class GetEventResponse(string id, string name, string date, string capacity, string cost, List<string> attractions)
{
    public List<string> Attractions { get; } = attractions;
    public string Capacity { get; } = capacity;
    public string Cost { get; } = cost;
    public string Date { get; } = date;
    public string Id { get; } = id;
    public string Name { get; } = name;
}
