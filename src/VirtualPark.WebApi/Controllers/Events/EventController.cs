using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Events.Models;
using VirtualPark.BusinessLogic.Events.Services;
using VirtualPark.WebApi.Controllers.Events.ModelsIn;
using VirtualPark.WebApi.Controllers.Events.ModelsOut;

namespace VirtualPark.WebApi.Controllers.Events;

[ApiController]
public sealed class EventController(IEventService eventService) : ControllerBase
{
    private readonly IEventService _eventService = eventService;

    [HttpPost("v1/events")]
    public CreateEventResponse CreateEvent(CreateEventRequest request)
    {
        var args = request.ToArgs();
        var id = _eventService.Create(args);
        return new CreateEventResponse(id.ToString());
    }
}
