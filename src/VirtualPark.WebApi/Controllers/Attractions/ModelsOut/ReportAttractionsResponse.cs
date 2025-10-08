namespace VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

public class ReportAttractionsResponse(string name)
{
    public string Name { get; } = name;
}
