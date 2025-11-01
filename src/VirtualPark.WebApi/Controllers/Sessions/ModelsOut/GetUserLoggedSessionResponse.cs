namespace VirtualPark.WebApi.Controllers.Sessions.ModelsOut;

public class GetUserLoggedSessionResponse(string id, string? visitorId, List<string> roleNames)
{
    public string? VisitorId { get; } = visitorId;
    public string Id { get; } = id;
    public List<string> Roles { get; } = roleNames;
}
