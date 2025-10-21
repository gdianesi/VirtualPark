namespace VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

public class CreateAttractionResponse(string id)
{
    public string? Id { get; init; } = id;
}
