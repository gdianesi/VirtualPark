using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Incidences.Models;

public sealed class IncidenceArgs(string typeIncidence, string description)
{
    public Guid TypeIncidence { get; init; } = ValidationServices.ValidateAndParseGuid(typeIncidence);
    public string Description { get; set; } = ValidationServices.ValidateNullOrEmpty(description);
}
