namespace VirtualPark.WebApi.Controllers.Incidences.ModelsOut;

public class CreateIncidenceResponse(string id)
{
    public string Id { get; init; } = id;
}
