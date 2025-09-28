using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Incidences.Models;

public sealed class IncidenceArgs(string typeIncidence, string description, string start, string end, string attractionId, string active)
{
    public Guid TypeIncidence { get; init; } = ValidationServices.ValidateAndParseGuid(typeIncidence);
    public string Description { get; init; } = ValidationServices.ValidateNullOrEmpty(description);
    public DateTime Start { get; init; } = ValidationServices.ValidateDateTime(start);
    public DateTime End { get; init; } = ValidationServices.ValidateDateTime(end);
    public Guid AttractionId { get; init; } = ValidationServices.ValidateAndParseGuid(attractionId);
    public bool Active { get; set; } = ValidationServices.ValidateAndParseBool(active);
}
