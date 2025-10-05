namespace VirtualPark.WebApi.Controllers.Users.ModelsIn;

public class CreateUserRequest
{
    public string? Name { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }
    public List<string>? RolesIds { get; init; }
}
