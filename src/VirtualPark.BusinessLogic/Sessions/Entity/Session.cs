using VirtualPark.BusinessLogic.Users.Entity;

namespace VirtualPark.BusinessLogic.Sessions.Entity;

public class Session
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    public Guid Token { get; } = Guid.NewGuid();
}
