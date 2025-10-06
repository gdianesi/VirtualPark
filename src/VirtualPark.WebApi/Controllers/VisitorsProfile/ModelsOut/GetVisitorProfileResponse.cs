namespace VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsOut;

public class GetVisitorProfileResponse(string id, string dateOfBirth)
{
    public string Id { get; } = id;
    public string DateOfBirth { get; set; } = dateOfBirth;
}
