using VirtualPark.BusinessLogic.TypeIncidences.Entity;

namespace VirtualPark.WebApi.Controllers.TypeIncidences.ModelsOut;

public class GetTypeIncidenceResponse(TypeIncidence typeIncidence)
{
    public string Id { get; } = typeIncidence.Id.ToString();
    public string Type { get; } = typeIncidence.Type;
}
