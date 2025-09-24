using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Attractions.Models;

public sealed class AttractionArgs(string type, string name, string miniumAge, string capacity, string description)
{
    public string Type { get; init; } = type;
    public string Name { get; init; } = name;
    public int MiniumAge { get; init; } = ValidationServices.ValidateAndParseInt(miniumAge);
    public int Capacity { get; init; } = ValidationServices.ValidateAndParseInt(capacity);
    public string Description { get; init; } = description;
}
