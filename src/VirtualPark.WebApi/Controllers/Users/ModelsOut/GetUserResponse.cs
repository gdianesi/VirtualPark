namespace VirtualPark.WebApi.Controllers.Users.ModelsOut;

public class GetUserResponse(string id, string name, string lastName, string email)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string LastName { get; } = lastName;
    public string Email { get; } = email;
}
