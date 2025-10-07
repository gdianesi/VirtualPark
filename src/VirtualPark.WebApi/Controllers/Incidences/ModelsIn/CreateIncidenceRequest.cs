using VirtualPark.BusinessLogic.Incidences.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.Incidences.ModelsIn;

public class CreateIncidenceRequest
{
    public string? TypeId { get; init; }
    public string? Description { get; init; }
    public string? Start { get; init; }
    public string? End { get; init; }
    public string? AttractionId { get; init; }
    public string? Active { get; init; }

    public IncidenceArgs ToArgs()
    {
        var incidenceArgs = new IncidenceArgs(ValidationServices.ValidateNullOrEmpty(TypeId),
            ValidationServices.ValidateNullOrEmpty(Description),
            ValidationServices.ValidateNullOrEmpty(Start),
            ValidationServices.ValidateNullOrEmpty(End),
            ValidationServices.ValidateNullOrEmpty(AttractionId),
            ValidationServices.ValidateNullOrEmpty(Active));

        return incidenceArgs;
    }
}
