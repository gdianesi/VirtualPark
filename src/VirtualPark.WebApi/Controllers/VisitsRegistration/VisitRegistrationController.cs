using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.VisitRegistrations.Service;
using VirtualPark.WebApi.Controllers.VisitsScore.ModelsIn;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.VisitsRegistration;

public class VisitRegistrationController(IVisitRegistrationService svc) : ControllerBase
{
    private readonly IVisitRegistrationService _svc = svc;

    [HttpPost("visitRegistrations/scoreEvents")]
    [AuthorizationFilter]
    public IActionResult RecordScoreEvent([FromBody] VisitScoreRequest body)
    {
        var args = body.ToArgs();
        _svc.RecordVisitScore(args);
        return NoContent();
    }
}
