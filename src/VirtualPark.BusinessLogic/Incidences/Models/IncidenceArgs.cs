using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Incidences.Models;

public sealed class IncidenceArgs(string typeIncidence, string description, string start)
{
    public Guid TypeIncidence { get; init; } = ValidationServices.ValidateAndParseGuid(typeIncidence);
    public string Description { get; init; } = ValidationServices.ValidateNullOrEmpty(description);
    public DateTime StartDate { get; init; } = ValidationServices.ValidateDateOnly(start);
}
