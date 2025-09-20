namespace VirtualPark.BusinessLogic.Users.Entity;

public sealed class User
{
    public Guid Id { get; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; init; } = null!;

    public User()
    {
        Id = Guid.NewGuid();
    }
}
