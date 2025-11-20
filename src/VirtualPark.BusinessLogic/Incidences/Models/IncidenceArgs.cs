using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Incidences.Models;

public sealed class IncidenceArgs(string typeIncidence, string description, string start, string end, string attractionId, string active)
{
    public Guid TypeIncidence { get; } = ValidationServices.ValidateAndParseGuid(typeIncidence);
    public string Description { get; } = ValidationServices.ValidateNullOrEmpty(description);
    public DateTime Start { get; } = ValidationServices.ValidateDateTime(start);
    public DateTime End { get; } = ValidationServices.ValidateDateTime(end);
    public Guid AttractionId { get; } = ValidationServices.ValidateAndParseGuid(attractionId);
    public bool Active { get; } = ValidationServices.ValidateAndParseBool(active);
}
