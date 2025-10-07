namespace VirtualPark.WebApi.Controllers.TypeIncidences.ModelsOut;

public class GetTypeIncidenceResponse(string id, string type)
{
    public string Id { get; } = id;
    public string Type { get; } = type;
}
