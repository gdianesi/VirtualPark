namespace VirtualPark.BusinessLogic.Attractions.Models;

public sealed class AttractionArgs(string type, string name, string miniumAge, string capacity)
{
    public string Type { get; init; } = type;
    public string Name { get; init; } = name;
    public int MiniumAge { get; init; } = miniumAge;
    public int Capacity { get; init; } = capacity;
}
