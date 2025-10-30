namespace VirtualPark.WebApi.Controllers.Sessions.ModelsOut;

public class GetUserLoggedSessionResponse(string id, string? visitorId)
{
    public string? VisitorId { get; } = visitorId;
    public string Id { get; } = id;
}
