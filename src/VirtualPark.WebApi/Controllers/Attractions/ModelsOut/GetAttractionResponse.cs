namespace VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

public class GetAttractionResponse(string id, string name, string type, string miniumAge, string capacity, string description, List<string> eventsId, string available)
{
    public string? Id { get; } = id;
    public string? Name { get; } = name;
    public string? Type { get; } = type;
    public string? MiniumAge { get; } = miniumAge;
    public string? Capacity { get; } = capacity;
    public string? Description { get; } = description;
    public List<string>? EventIds { get; } = eventsId;
    public string? Available { get; } = available;
}
