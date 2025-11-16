using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitRegistrations.Service;
using VirtualPark.WebApi.Controllers.VisitsScore.ModelsIn;
using VirtualPark.WebApi.Filters.Authenticator;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.VisitsRegistration;

[ApiController]
[AuthenticationFilter]
public class VisitRegistrationController(IVisitRegistrationService svc) : ControllerBase
{
    private readonly IVisitRegistrationService _svc = svc;

    [HttpPost("visitRegistrations/scoreEvents/{token}")]
    [AuthorizationFilter]
    public IActionResult RecordScoreEvent([FromBody] VisitScoreRequest body, string token)
    {
        var args = body.ToArgs();
        var sessionToken = ValidationServices.ValidateAndParseGuid(token);
        _svc.RecordVisitScore(args, sessionToken);
        return NoContent();
    }

    [HttpPost("visitRegistrations/{visitorId}/currentAttraction/{attractionId}")]
    [AuthorizationFilter]
    public IActionResult UpToAttraction(string visitorId, string attractionId)
    {
        var vId = ValidationServices.ValidateAndParseGuid(visitorId);
        var aId = ValidationServices.ValidateAndParseGuid(attractionId);

        _svc.UpToAttraction(vId, aId);
        return NoContent();
    }

    [HttpPost("visitRegistrations/{visitorId}/currentAttraction")]
    [AuthorizationFilter]
    public IActionResult DownToAttraction(string visitorId)
    {
        var vId = ValidationServices.ValidateAndParseGuid(visitorId);

        _svc.DownToAttraction(vId);

        return NoContent();
    }

    [HttpGet("visitRegistrations/{visitorId}/availableAttractions")]
    [AuthorizationFilter]
    public IActionResult GetAttractionsForTicket(string visitorId)
    {
        var parsedVisitorId = ValidationServices.ValidateAndParseGuid(visitorId);

        var attractions = _svc.GetAttractionsForTicket(parsedVisitorId);

        return Ok(attractions);
    }

    [HttpGet("visitRegistrations/attractions/{attractionId}/visitors")]
    [AuthorizationFilter]
    public IActionResult GetVisitorsInAttraction(string attractionId)
    {
        var parsedAttractionId = ValidationServices.ValidateAndParseGuid(attractionId);

        var visitors = _svc.GetVisitorsInAttraction(parsedAttractionId);

        return Ok(visitors);
    }
}
