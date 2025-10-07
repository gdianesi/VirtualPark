namespace VirtualPark.WebApi.Controllers.Incidences.ModelsOut;

public class GetIncidenceResponse(string id, string typeId, string description, string start, string end, string attractionId, string active)
{
    public string Id { get; init; } = id;
    public string TypeId { get; init; } = typeId;
    public string Description { get; init; } = description;
    public string Start { get; init; } = start;
    public string End { get; init; } = end;
    public string AttractionId { get; init; } = attractionId;
    public string Active { get; init; } = active;
}
