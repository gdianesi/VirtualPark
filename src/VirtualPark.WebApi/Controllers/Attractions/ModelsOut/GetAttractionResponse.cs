namespace VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

public class GetAttractionResponse(string id, string name, string type, string miniumAge, string capacity, string description, List<string> eventsId, string available)
{
    public string? Id { get; init; } = id;
    public string? Name { get; init; } = name;
    public string? Type { get; init; } = type;
    public string? MiniumAge { get; init; } = miniumAge;
    public string? Capacity { get; init; } = capacity;
    public string? Description { get; init; } = description;
    public List<string>? EventIds { get; init; } = eventsId;
    public string? Available { get; init; } = available;
}
