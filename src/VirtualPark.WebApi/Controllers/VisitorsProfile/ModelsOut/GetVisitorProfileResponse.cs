namespace VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsOut;

public class GetVisitorProfileResponse(string id, string dateOfBirth, string membership, string score)
{
    public string Id { get; } = id;
    public string DateOfBirth { get; } = dateOfBirth;
    public string Membership { get; } = membership;
    public string Score { get; } = score;
}
