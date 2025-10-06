namespace VirtualPark.WebApi.Controllers.Users.ModelsOut;

public class GetUserResponse(string id, string name, string lastName, string email, List<string> roles)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string LastName { get; } = lastName;
    public string Email { get; } = email;
    public List<string> Roles { get; } = roles;
}
