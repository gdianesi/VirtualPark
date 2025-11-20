using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Service;
using VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsOut;
using VirtualPark.WebApi.Filters.Authenticator;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.VisitorsProfile;

[ApiController]
[AuthenticationFilter]
[Route("visitorProfiles")]
public sealed class VisitorProfileController(IVisitorProfileService visitorProfileServiceService) : ControllerBase
{
    private readonly IVisitorProfileService _visitorProfileServiceService = visitorProfileServiceService;

    [HttpGet("{id}")]
    [AuthorizationFilter]
    public GetVisitorProfileResponse GetVisitorProfileById(string id)
    {
        var visitorId = ValidationServices.ValidateAndParseGuid(id);

        var vp = _visitorProfileServiceService.Get(visitorId)!;

        return new GetVisitorProfileResponse(vp);
    }

    [HttpGet]
    [AuthorizationFilter]
    public List<GetVisitorProfileResponse> GetAllVisitorProfiles()
    {
        return _visitorProfileServiceService
            .GetAll()
            .Select(vp => new GetVisitorProfileResponse(vp))
            .ToList();
    }
}
