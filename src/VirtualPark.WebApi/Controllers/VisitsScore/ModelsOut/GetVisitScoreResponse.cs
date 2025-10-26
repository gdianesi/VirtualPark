namespace VirtualPark.WebApi.Controllers.VisitsScore.ModelsOut;

public class GetVisitScoreResponse(string id, string origin)
{
    public string Id { get; } = id;
    public string Origin { get; } = origin;
}
