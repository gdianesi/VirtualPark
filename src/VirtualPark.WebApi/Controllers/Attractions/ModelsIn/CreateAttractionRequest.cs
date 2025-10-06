namespace VirtualPark.WebApi.Controllers.Attractions.ModelsIn;

public sealed class CreateAttractionRequest
{
    public string? Name { get; init; }
    public string? TypeId { get; init; }
    public string? MiniumAge { get; set; }
}
