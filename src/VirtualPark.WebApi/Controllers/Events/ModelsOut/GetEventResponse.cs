namespace VirtualPark.WebApi.Controllers.Events.ModelsOut;

public sealed class GetEventResponse(string id, string name, string date, string capacity, string cost)
{
    public string Capacity { get; } = capacity;
    public string Date { get; set; } = date;
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string Cost { get; } = cost;
}
