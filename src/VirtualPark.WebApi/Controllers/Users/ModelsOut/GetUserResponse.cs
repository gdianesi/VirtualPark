using VirtualPark.BusinessLogic.Users.Entity;

namespace VirtualPark.WebApi.Controllers.Users.ModelsOut;

public class GetUserResponse(User user)
{
    public string Id { get; } = user.Id.ToString();
    public string Name { get; } = user.Name;
    public string LastName { get; } = user.LastName;
    public string Email { get; } = user.Email;
    public List<string> Roles { get; } = user.Roles?
            .Select(r => r.Id.ToString())
            .ToList() ?? [];
    public string? VisitorProfileId { get; } = user.VisitorProfileId?.ToString();
}
