using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsOut;

public class GetVisitorProfileResponse(VisitorProfile vp)
{
    public string Id { get; } = vp.Id.ToString();
    public string DateOfBirth { get; } = vp.DateOfBirth.ToString("yyyy-MM-dd");
    public string Membership { get; } = vp.Membership.ToString();
    public string Score { get; } = vp.Score.ToString();
    public string NfcId { get; } = vp.NfcId.ToString();
    public string PointsAvailable { get; } = vp.PointsAvailable.ToString();
}
