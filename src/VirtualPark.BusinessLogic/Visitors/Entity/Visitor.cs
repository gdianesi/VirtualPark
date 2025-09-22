namespace VirtualPark.BusinessLogic.Visitors.Entity;

public class Visitor
{
    public Guid Id { get; init; } =  Guid.NewGuid();
    public string Name { get; set; } = null!;
    public object LastName { get; set; } = null!;
    public object Email { get; set; } = null!;
    public object PasswordHash { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public object Score { get; set; } = 0;
}
