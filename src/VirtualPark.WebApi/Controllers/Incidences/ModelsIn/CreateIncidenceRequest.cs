namespace VirtualPark.WebApi.Controllers.Incidences.ModelsIn;

public class CreateIncidenceRequest
{
    public string? TypeId { get; init; }
    public string? Description { get; init; }
    public string? Start { get; init; }
    public string? End { get; init; }
    public string? AttractionId { get; init; }
    public string? Active { get; set; }
}
