namespace VirtualPark.BusinessLogic.Permissions.Entity;

public class Permission
{
    public Guid Id { get; }
    public string Description { get; set; } = null!;
    public string Key { get; set; } = null!;

    public Permission()
    {
        Id = Guid.NewGuid();
    }
}
