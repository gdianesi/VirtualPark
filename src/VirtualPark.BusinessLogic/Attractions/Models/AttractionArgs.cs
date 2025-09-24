namespace VirtualPark.BusinessLogic.Attractions.Models;

public class AttractionArgs
{
    public string Type { get; init; }

    public AttractionArgs(string type)
    {
        Type = type;
    }
}
