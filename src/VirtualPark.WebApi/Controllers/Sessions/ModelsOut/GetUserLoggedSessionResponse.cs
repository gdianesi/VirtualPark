namespace VirtualPark.WebApi.Controllers.Sessions.ModelsOut;

public class GetUserLoggedSessionResponse(string id)
{
    public string Id { get; } = id;
}
