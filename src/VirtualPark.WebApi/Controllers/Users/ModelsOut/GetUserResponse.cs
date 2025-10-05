namespace VirtualPark.WebApi.Controllers.Users.ModelsOut;

public class GetUserResponse(string id, string name)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
}
