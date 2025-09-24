namespace VirtualPark.BusinessLogic.Attractions.Models;

public class AttractionArgs(string type)
{
    public string Type { get; init; } = type;
}
