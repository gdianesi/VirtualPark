namespace VirtualPark.BusinessLogic.Users.Entity;

public sealed class User
{
    public Guid Id { get; }
    public string Name { get; init; } = null!;

    public User()
    {
        Id = Guid.NewGuid();
    }
}
