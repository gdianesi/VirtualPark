using VirtualPark.BusinessLogic.Users.Entity;

namespace VirtualPark.BusinessLogic.Sessions.Entity;

public class Session
{
    public Guid Id { get; } = Guid.NewGuid();
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    public Guid Token { get; } = Guid.NewGuid();
}
