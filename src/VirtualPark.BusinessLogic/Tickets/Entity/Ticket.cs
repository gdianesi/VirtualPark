namespace VirtualPark.BusinessLogic.Tickets.Entity;

public sealed class Ticket
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime Date { get; set; }
}
