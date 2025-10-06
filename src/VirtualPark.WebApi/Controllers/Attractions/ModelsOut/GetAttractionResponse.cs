namespace VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

public class GetAttractionResponse
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? TypeId { get; init; }
    public string? MiniumAge { get; init; }
    public string? Capacity { get; init; }
    public string? Description { get; init; }
    public List<string>? EventIds { get; init; }
    public string? Available { get; set; }
}
