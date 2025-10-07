namespace VirtualPark.WebApi.Controllers.Incidences.ModelsOut;

public class GetIncidenceResponse(string id, string typeId, string description, string start, string end)
{
    public string Id { get; init; } = id;
    public string TypeId { get; init; } = typeId;
    public string Description { get; init; } = description;
    public string Start { get; init; } = start;
    public string End { get; init; } = end;
}
