using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Events.Models;

public sealed class EventsArgs(string name, string date, int capacity, int cost, List<string>? attractionsIds)
{
    public string Name { get; } = ValidationServices.ValidateNullOrEmpty(name);
    public DateOnly Date { get; } = ValidationServices.ValidateDateOnly(date);
    public int Capacity { get; } = ValidationServices.ValidatePositive(capacity);
    public int Cost { get; } = ValidationServices.ValidatePositive(cost);
    public List<Guid> AttractionIds { get; } = ValidationServices.ValidateAndParseGuidAttractionsList(attractionsIds);
}
