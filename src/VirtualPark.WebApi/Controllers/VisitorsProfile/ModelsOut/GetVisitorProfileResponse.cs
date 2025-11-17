namespace VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsOut;

public class GetVisitorProfileResponse(string id, string dateOfBirth, string membership, string score, string nfcId, string pointsAvailable)
{
    public string Id { get; } = id;
    public string DateOfBirth { get; } = dateOfBirth;
    public string Membership { get; } = membership;
    public string Score { get; } = score;
    public string NfcId { get; } = nfcId;
    public string PointsAvailable { get; } = pointsAvailable;
}
