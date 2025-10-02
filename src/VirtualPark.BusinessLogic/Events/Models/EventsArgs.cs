using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Events.Models;

public sealed class EventsArgs(string name, string date, int capacity, int cost, List<string> attractionsIds)
{
    public string Name { get; init; } = ValidationServices.ValidateNullOrEmpty(name);
    public DateOnly Date { get; init; } = ValidationServices.ValidateDateOnly(date);
    public int Capacity { get; set; } = ValidationServices.ValidatePositive(capacity);
    public int Cost { get; set; } = ValidationServices.ValidatePositive(cost);
    public List<Guid> AttractionIds { get; init; } = ValidationServices.ValidateAndParseGuidList(attractionsIds);
}
