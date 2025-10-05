namespace VirtualPark.WebApi.Controllers.Users.ModelsOut;

public class GetUserResponse(string id)
{
    public string Id { get; } = id;
}
