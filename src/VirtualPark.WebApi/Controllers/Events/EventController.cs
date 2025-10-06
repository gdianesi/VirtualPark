using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Events.Models;
using VirtualPark.BusinessLogic.Events.Services;
using VirtualPark.BusinessLogic.Validations.Services;
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
        EventsArgs args = request.ToArgs();
        Guid eventId = _eventService.Create(args);
        return new CreateEventResponse(eventId.ToString());
    }

    [HttpGet("v1/events/{id}")]
    public GetEventResponse GetEventById(string id)
    {
        var eventId = ValidationServices.ValidateAndParseGuid(id);
        var ev = _eventService.Get(eventId)!;

        return new GetEventResponse(
            id: ev.Id.ToString(),
            name: ev.Name,
            date: ev.Date.ToString("yyyy-MM-dd"),
            capacity: ev.Capacity.ToString(),
            cost: ev.Cost.ToString(),
            attractions: ev.Attractions.Select(a => a.Id.ToString()).ToList());
    }

    [HttpGet("v1/events")]
    public List<GetEventResponse> GetAllEvents()
    {
        var events = _eventService.GetAll();

        return events
            .Select(ev => new GetEventResponse(
                id: ev.Id.ToString(),
                name: ev.Name,
                date: ev.Date.ToString("yyyy-MM-dd"),
                capacity: ev.Capacity.ToString(),
                cost: ev.Cost.ToString(),
                attractions: ev.Attractions
                    .Select(a => a.Id.ToString())
                    .ToList()))
            .ToList();
    }
}
