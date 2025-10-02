using VirtualPark.BusinessLogic.Permissions.Entity;
namespace VirtualPark.BusinessLogic.Roles.Entity;

public class Role
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<Permission> Permissions { get; set; } = [];
}
