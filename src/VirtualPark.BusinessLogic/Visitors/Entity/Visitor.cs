namespace VirtualPark.BusinessLogic.Visitors.Entity;

public class Visitor
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public int Score { get; private set; } = 0;
    public Membership Membership { get; private set; } = Membership.Standard;
}
