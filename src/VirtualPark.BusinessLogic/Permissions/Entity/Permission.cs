using VirtualPark.BusinessLogic.Roles.Entity;

namespace VirtualPark.BusinessLogic.Permissions.Entity;

public class Permission
{
    public Permission()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public string Description { get; set; } = null!;
    public string Key { get; set; } = null!;
    public List<Role> Roles { get; } = [];
}
