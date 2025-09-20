namespace VirtualPark.BusinessLogic.Users.Entity;

public sealed class User
{
    public Guid Id { get; init; }

    public User()
    {
        Id = Guid.NewGuid();
    }
}
