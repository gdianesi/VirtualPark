using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Users.Service;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitRegistrations.Service;
using VirtualPark.WebApi.Controllers.Users.ModelsOut;
using VirtualPark.WebApi.Controllers.VisitsRegistration.ModelsOut;
using VirtualPark.WebApi.Controllers.VisitsScore.ModelsIn;
using VirtualPark.WebApi.Filters.Authenticator;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.VisitsRegistration;

[ApiController]
[AuthenticationFilter]
[Route("visitRegistrations")]

public class VisitRegistrationController(IVisitRegistrationService svc, IUserService userService) : ControllerBase
{
    private readonly IVisitRegistrationService _svc = svc;
    private readonly IUserService _userService = userService;

    [HttpPost("scoreEvents")]
    [AuthorizationFilter]
    public IActionResult RecordScoreEvent([FromBody] VisitScoreRequest body)
    {
        var args = body.ToArgs();
        _svc.RecordVisitScore(args);
        return NoContent();
    }

    [HttpPost("{visitorId}/currentAttraction/{attractionId}")]
    [AuthorizationFilter]
    public IActionResult UpToAttraction(string visitorId, string attractionId)
    {
        var vId = ValidationServices.ValidateAndParseGuid(visitorId);
        var aId = ValidationServices.ValidateAndParseGuid(attractionId);

        _svc.UpToAttraction(vId, aId);
        return NoContent();
    }

    [HttpPost("{visitorId}/currentAttraction")]
    [AuthorizationFilter]
    public IActionResult DownToAttraction(string visitorId)
    {
        var vId = ValidationServices.ValidateAndParseGuid(visitorId);

        _svc.DownToAttraction(vId);

        return NoContent();
    }

    [HttpGet("{visitorId}/availableAttractions")]
    [AuthorizationFilter]
    public IActionResult GetAttractionsForTicket(string visitorId)
    {
        var parsedVisitorId = ValidationServices.ValidateAndParseGuid(visitorId);

        var attractions = _svc.GetAttractionsForTicket(parsedVisitorId);

        return Ok(attractions);
    }

    [HttpGet("attractions/{attractionId}/visitors")]
    [AuthorizationFilter]
    public IActionResult GetVisitorsInAttraction(string attractionId)
    {
        var aId = ValidationServices.ValidateAndParseGuid(attractionId);

        var visitorsInAttraction = _svc.GetVisitorsInAttraction(aId);

        var vpIds = visitorsInAttraction
            .Select(v => v.Visitor.Id)
            .ToList();

        var users = _userService.GetByVisitorProfileIds(vpIds);

        var result =
            (from v in visitorsInAttraction
             let vp = v.Visitor
             let user = users.SingleOrDefault(u => u.VisitorProfileId == vp.Id)
             where user != null
             select new VisitorInAttractionResponse
             {
                 VisitRegistrationId = v.VisitRegistrationId,
                 VisitorProfileId = vp.Id,
                 UserId = user.Id,
                 Name = user.Name,
                 LastName = user.LastName,
                 Score = vp.Score,
                 Membership = vp.Membership,
                 NfcId = vp.NfcId,
                 TicketType = v.TicketType
             }).ToList();

        return Ok(result);
    }

    [HttpGet("{visitorId}/today")]
    [AuthorizationFilter]
    public IActionResult GetVisitForToday(string visitorId)
    {
        var vId = ValidationServices.ValidateAndParseGuid(visitorId);

        var visit = _svc.GetTodayVisit(vId);

        return Ok(new VisitRegistrationTodayResponse(visit));
    }
}
