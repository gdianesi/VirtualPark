using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Incidences.Models;

public sealed class IncidenceArgs(string typeIncidence, string description, string start, string end)
{
    public Guid TypeIncidence { get; init; } = ValidationServices.ValidateAndParseGuid(typeIncidence);
    public string Description { get; init; } = ValidationServices.ValidateNullOrEmpty(description);
    public DateTime Start { get; init; } = ValidationServices.ValidateDateTime(start);
    public DateTime End { get; set; } = ValidationServices.ValidateDateTime(end);
}
