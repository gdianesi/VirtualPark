using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.WebApi.Controllers.ClockApp.ModelsOut;

namespace VirtualPark.WebApi.Controllers.ClockApp;

[ApiController]
public sealed class ClockAppController(IClockAppService clockAppService) : ControllerBase
{
    private readonly IClockAppService _clockAppService = clockAppService;

    [HttpGet("/clock")]
    public GetClockResponse GetClock()
    {
        var clock = _clockAppService.Get();
        return new GetClockResponse(
            dateSystem: clock.DateSystem.ToString("yyyy-MM-ddTHH:mm:ss"));
    }
}
