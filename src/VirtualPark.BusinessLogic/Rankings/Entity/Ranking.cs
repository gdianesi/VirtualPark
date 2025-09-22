using VirtualPark.BusinessLogic.Users.Entity;

namespace VirtualPark.BusinessLogic.Rankings.Entity;

public sealed class Ranking
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime Date { get; set; }
    public List<User> Entries { get; set; } = [];
}
