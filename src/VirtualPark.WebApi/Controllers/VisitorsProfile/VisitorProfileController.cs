using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Service;
using VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsOut;

namespace VirtualPark.WebApi.Controllers.VisitorsProfile;

public sealed class VisitorProfileController(IVisitorProfileService visitorProfileServiceService) : ControllerBase
{
    private readonly IVisitorProfileService _visitorProfileServiceService = visitorProfileServiceService;

    [HttpGet("visitorProfiles/{id}")]
    public GetVisitorProfileResponse GetVisitorProfileById(string id)
    {
        var visitorId = ValidationServices.ValidateAndParseGuid(id);

        var vp = _visitorProfileServiceService.Get(visitorId)!;

        return new GetVisitorProfileResponse(
            id: vp.Id.ToString(),
            dateOfBirth: vp.DateOfBirth.ToString("yyyy-MM-dd"),
            membership: vp.Membership.ToString(),
            score: vp.Score.ToString(),
            nfcId: vp.NfcId.ToString());
    }

    [HttpGet("visitorProfiles")]
    public List<GetVisitorProfileResponse> GetAllVisitorProfiles()
    {
        return _visitorProfileServiceService
            .GetAll()
            .Select(vp => new GetVisitorProfileResponse(
                id: vp.Id.ToString(),
                dateOfBirth: vp.DateOfBirth.ToString("yyyy-MM-dd"),
                membership: vp.Membership.ToString(),
                score: vp.Score.ToString(),
                nfcId: vp.NfcId.ToString()))
            .ToList();
    }
}
