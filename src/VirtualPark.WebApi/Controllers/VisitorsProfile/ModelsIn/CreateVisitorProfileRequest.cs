using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;

namespace VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsIn;

public class CreateVisitorProfileRequest
{
    public string? DateOfBirth { get; init; }
    public string? Membership { get; init; }
    public string? Score { get; init; }

    public VisitorProfileArgs ToArgs()
    {
        return new VisitorProfileArgs(ValidationServices.ValidateNullOrEmpty(DateOfBirth),
            ValidationServices.ValidateNullOrEmpty(Membership),
            ValidationServices.ValidateNullOrEmpty(Score));
    }
}
