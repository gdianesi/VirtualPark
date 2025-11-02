namespace VirtualPark.WebApi.Controllers.Incidences.ModelsOut;

public class GetIncidenceResponse(string id, string typeId, string description, string start, string end, string attractionId, string active)
{
    public string Id { get; } = id;
    public string TypeId { get; } = typeId;
    public string Description { get; } = description;
    public string Start { get; } = start;
    public string End { get; } = end;
    public string AttractionId { get; } = attractionId;
    public string Active { get; } = active;
}
