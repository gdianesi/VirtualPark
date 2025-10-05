namespace VirtualPark.WebApi.Controllers.Users.ModelsOut;

public class CreateUserResponse(string id)
{
    public string Id { get; } = id;
}
