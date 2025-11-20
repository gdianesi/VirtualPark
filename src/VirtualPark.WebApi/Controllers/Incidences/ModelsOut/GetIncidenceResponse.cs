using VirtualPark.BusinessLogic.Incidences.Entity;

namespace VirtualPark.WebApi.Controllers.Incidences.ModelsOut;

public class GetIncidenceResponse(Incidence? incidence)
{
    public string Id { get; } = incidence.Id.ToString();
    public string TypeId { get; } = incidence.Type.Id.ToString();
    public string Description { get; } = incidence.Description;
    public string Start { get; } = incidence.Start.ToString();
    public string End { get; } = incidence.End.ToString();
    public string AttractionId { get; } = incidence.AttractionId.ToString();
    public string Active { get; } = incidence.Active.ToString();
    public string ManualOverride { get; } = incidence.ManualOverride.ToString();
}
