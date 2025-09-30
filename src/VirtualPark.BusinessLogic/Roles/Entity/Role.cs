using VirtualPark.BusinessLogic.Permissions.Entity;
namespace VirtualPark.BusinessLogic.Roles.Entity;

public class Role
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public List<Permission> Permissions { get; } = [];
}
