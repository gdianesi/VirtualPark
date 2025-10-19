namespace VirtualPark.BusinessLogic.RewardRedemptions.Entity;

public sealed class RewardRedemption
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid RewardId { get; init; }
    public Guid VisitorId { get; init; }
    public DateOnly Date { get; init; }
    public int PointsSpent { get; set; }
}
