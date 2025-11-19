using VirtualPark.BusinessLogic.Attractions.Entity;

namespace VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

public class GetAttractionResponse
{
    public string? Id { get; }
    public string? Name { get; }
    public string? Type { get; }
    public string? MiniumAge { get; }
    public string? Capacity { get; }
    public string? Description { get; }
    public List<string>? EventIds { get; }
    public string? Available { get; }

    public GetAttractionResponse(Attraction a)
    {
        Id = a.Id.ToString();
        Name = a.Name;
        Type = a.Type.ToString();
        MiniumAge = a.MiniumAge.ToString();
        Capacity = a.Capacity.ToString();
        Description = a.Description;
        EventIds = a.Events.Select(e => e.Id.ToString()).ToList();
        Available = a.Available.ToString();
    }
}
