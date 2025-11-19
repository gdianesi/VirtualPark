using VirtualPark.BusinessLogic.Incidences.Entity;

namespace VirtualPark.WebApi.Controllers.Incidences.ModelsOut;

public class GetIncidenceResponse
{
    public string Id { get; }
    public string TypeId { get; }
    public string Description { get; }
    public string Start { get; }
    public string End { get; }
    public string AttractionId { get; }
    public string Active { get; }

    public GetIncidenceResponse(Incidence? incidence)
    {
        Id = incidence.Id.ToString();
        TypeId = incidence.Type.Id.ToString();
        Description = incidence.Description;
        Start = incidence.Start.ToString();
        End = incidence.End.ToString();
        AttractionId = incidence.AttractionId.ToString();
        Active = incidence.Active.ToString();
    }
}
