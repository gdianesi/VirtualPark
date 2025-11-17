namespace VirtualPark.BusinessLogic.VisitorsProfile.Entity;

public class VisitorProfile
{
    public Guid Id { get; init; }
    public DateOnly DateOfBirth { get; set; }
    public Membership Membership { get; set; }
    public int Score { get; set; } = 0;
    public Guid NfcId { get; set; }
    public int PointsAvailable { get; set; } = 0;
    public VisitorProfile()
    {
        Id = Guid.NewGuid();
        NfcId = Guid.NewGuid();
    }
}
