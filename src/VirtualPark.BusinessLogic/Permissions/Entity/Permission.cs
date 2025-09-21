namespace VirtualPark.BusinessLogic.Permissions.Entity;
using Roles.Entity;
public class Permission
{
    public Guid Id { get; }
    public string Description { get; set; } = null!;
    public string Key { get; set; } = null!;
    public List<Role> Roles { get; set; } = new();

    public Permission()
    {
        Id = Guid.NewGuid();
    }
}
