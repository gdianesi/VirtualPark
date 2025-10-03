using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Attractions.Models;

public sealed class AttractionArgs(string type, string name, string miniumAge, string capacity, string description, string currentVisitor, string available)
{
    public AttractionType Type { get; init; } = ValidationServices.ValidateAndParseAttractionType(type);
    public string Name { get; init; } = ValidationServices.ValidateNullOrEmpty(name);
    public int MiniumAge { get; init; } = ValidationServices.ValidateAndParseInt(miniumAge);
    public int Capacity { get; init; } = ValidationServices.ValidateAndParseInt(capacity);
    public string Description { get; init; } = description;
    public int CurrentVisitor { get; init; } = ValidationServices.ValidateAndParseInt(currentVisitor);
    public bool Available { get; init; } = ValidationServices.ValidateAndParseBool(available);
}
