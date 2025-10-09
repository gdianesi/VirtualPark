using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.WebApi.Controllers.ClockApp.ModelsIn;
using VirtualPark.WebApi.Controllers.ClockApp.ModelsOut;
using VirtualPark.WebApi.Filters.Authenticator;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.ClockApp;

[ApiController]
[AuthenticationFilter]
public sealed class ClockAppController(IClockAppService clockAppService) : ControllerBase
{
    private readonly IClockAppService _clockAppService = clockAppService;

    [HttpGet("/clock")]
    public GetClockResponse GetClock()
    {
        var clock = _clockAppService.Get();
        return MapToResponse(clock);
    }

    private static GetClockResponse MapToResponse(BusinessLogic.ClocksApp.Entity.ClockApp clock)
    {
        return new GetClockResponse(
            dateSystem: clock.DateSystem.ToString("yyyy-MM-ddTHH:mm:ss"));
    }

    [HttpPut("/clock")]
    [AuthorizationFilter]
    public void UpdateClock(UpdateClockRequest request)
    {
        var args = request.ToArgs();
        _clockAppService.Update(args);
    }
}
