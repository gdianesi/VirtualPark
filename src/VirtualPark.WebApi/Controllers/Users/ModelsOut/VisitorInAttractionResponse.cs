using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.WebApi.Controllers.Users.ModelsOut;

public class VisitorInAttractionResponse
{
    public Guid VisitRegistrationId { get; init; }
    public Guid VisitorProfileId { get; init; }
    public Guid UserId { get; init; }
    public string Name { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public int Score { get; init; }
    public Membership Membership { get; init; }
    public Guid NfcId { get; init; }
}
