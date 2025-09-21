namespace VirtualPark.BusinessLogic.Roles.Entity;

public class Role
{
    public Role()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
}
