namespace VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

public class GetAttractionResponse
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? TypeId { get; init; }
    public string? MiniumAge { get; set; }
}
