using VirtualPark.BusinessLogic.Users.Entity;

namespace VirtualPark.WebApi.Controllers.Sessions.ModelsOut;

public class GetUserLoggedSessionResponse(User? user)
{
    public string Id { get; } = user.Id.ToString();
    public string? VisitorId { get; } = user.VisitorProfileId?.ToString();
    public List<string> Roles { get; } = user.Roles.Select(r => r.Name).ToList();
}
