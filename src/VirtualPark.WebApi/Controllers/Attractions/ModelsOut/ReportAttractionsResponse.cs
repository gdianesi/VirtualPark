namespace VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

public class ReportAttractionsResponse(string name, string visits)
{
    public string Name { get; } = name;
    public string Visits { get; } = visits;
}
