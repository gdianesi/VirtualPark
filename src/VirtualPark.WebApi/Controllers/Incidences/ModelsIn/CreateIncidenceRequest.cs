namespace VirtualPark.WebApi.Controllers.Incidences.ModelsIn;

public class CreateIncidenceRequest
{
    public string? TypeId { get; init; }
    public string? Description { get; set; }
}
