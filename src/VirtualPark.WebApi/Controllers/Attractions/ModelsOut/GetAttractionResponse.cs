using VirtualPark.BusinessLogic.Attractions.Entity;

namespace VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

public class GetAttractionResponse(Attraction? a)
{
    public string? Id { get; } = a.Id.ToString();
    public string? Name { get; } = a.Name;
    public string? Type { get; } = a.Type.ToString();
    public string? MiniumAge { get; } = a.MiniumAge.ToString();
    public string? Capacity { get; } = a.Capacity.ToString();
    public string? Description { get; } = a.Description;
    public List<string>? EventIds { get; } = a.Events.Select(e => e.Id.ToString()).ToList();
    public string? Available { get; } = a.Available.ToString();
}
