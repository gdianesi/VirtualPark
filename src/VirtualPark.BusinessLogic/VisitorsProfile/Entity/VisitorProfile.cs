namespace VirtualPark.BusinessLogic.VisitorsProfile.Entity;

public sealed class VisitorProfile
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateOnly DateOfBirth { get; set; }
    public Membership Membership { get; set; }
    public int Score { get; set; } = 0;
    public Guid NfcId { get; set; } = Guid.NewGuid();
    public int PointsAvailable { get; set; } = 0;
}
