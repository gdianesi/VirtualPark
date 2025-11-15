using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitRegistrations.Service;
using VirtualPark.WebApi.Controllers.VisitsScore.ModelsIn;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.VisitsRegistration;

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

    [HttpPost("visitRegistrations/{visitId}/currentAttraction/{attractionId}")]
    [AuthorizationFilter]
    public IActionResult UpToAttraction(string visitId, string attractionId)
    {
        var vId = ValidationServices.ValidateAndParseGuid(visitId);
        var aId = ValidationServices.ValidateAndParseGuid(attractionId);

        _svc.UpToAttraction(vId, aId);
        return NoContent();
    }

    [HttpPost("visitRegistrations/{visitId}/currentAttraction")]
    [AuthorizationFilter]
    public IActionResult DownToAttraction(string visitId)
    {
        var parsedVisitId = ValidationServices.ValidateAndParseGuid(visitId);

        _svc.DownToAttraction(parsedVisitId);

        return NoContent();
    }
}
