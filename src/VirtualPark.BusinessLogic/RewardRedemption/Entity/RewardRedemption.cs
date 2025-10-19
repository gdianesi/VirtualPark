namespace VirtualPark.BusinessLogic.RewardRedemption.Entity;

public sealed class RewardRedemption
{
    public Guid RewardId { get; init; }
    public Guid VisitorId { get; init; }
    public DateOnly Date { get; init; }
}
