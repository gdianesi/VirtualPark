using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Attractions.Models;

public sealed class AttractionArgs(string type, string name, string miniumAge, string capacity, string description, string currentVisitor, string available)
{
    public AttractionType Type { get; } = ValidationServices.ValidateAndParseAttractionType(type);
    public string Name { get; } = ValidationServices.ValidateNullOrEmpty(name);
    public int MiniumAge { get; } = ValidationServices.ValidateAndParseInt(miniumAge);
    public int Capacity { get; } = ValidationServices.ValidateAndParseInt(capacity);
    public string Description { get; } = description;
    public int CurrentVisitor { get; } = ValidationServices.ValidateAndParseInt(currentVisitor);
    public bool Available { get; } = ValidationServices.ValidateAndParseBool(available);
}
