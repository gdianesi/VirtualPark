using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Events.Models;
using VirtualPark.BusinessLogic.Events.Services;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Events.ModelsIn;
using VirtualPark.WebApi.Controllers.Events.ModelsOut;
using VirtualPark.WebApi.Filters.Authenticator;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.Events;

[ApiController]
[AuthenticationFilter]
[Route("events")]
public sealed class EventController(IEventService eventService) : ControllerBase
{
    private readonly IEventService _eventService = eventService;

    [HttpPost]
    [AuthorizationFilter]
    public CreateEventResponse CreateEvent([FromBody] CreateEventRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        EventsArgs args = request.ToArgs();
        Guid eventId = _eventService.Create(args);
        return new CreateEventResponse(eventId.ToString());
    }

    [HttpGet("{id}")]
    [AuthorizationFilter]
    public GetEventResponse GetEventById(string id)
    {
        var eventId = ValidationServices.ValidateAndParseGuid(id);
        var ev = _eventService.Get(eventId)!;
        if(ev == null)
        {
            throw new InvalidOperationException($"Event with id {id} not found.");
        }

        return new GetEventResponse(
            id: ev.Id.ToString(),
            name: ev.Name,
            date: ev.Date.ToString("yyyy-MM-dd"),
            capacity: ev.Capacity.ToString(),
            cost: ev.Cost.ToString(),
            attractions: ev.Attractions.Select(a => a.Id.ToString()).ToList(),
            ticketsSold:
            ev.Tickets?.Count.ToString() ?? "0");
    }

    [HttpGet]
    [AuthorizationFilter]
    public List<GetEventResponse> GetAllEvents()
    {
        return _eventService
            .GetAll()
            .Select(MapToResponse)
            .ToList();
    }

    private static GetEventResponse MapToResponse(Event ev)
    {
        var attractions = ev.Attractions
            .Select(a => a.Id.ToString())
            .ToList();

        return new GetEventResponse(
            id: ev.Id.ToString(),
            name: ev.Name,
            date: ev.Date.ToString("yyyy-MM-dd"),
            capacity: ev.Capacity.ToString(),
            cost: ev.Cost.ToString(),
            attractions: attractions,
            ticketsSold: ev.Tickets?.Count.ToString() ?? "0");
    }

    [HttpDelete("{id}")]
    [AuthorizationFilter]
    public void DeleteEvent(string id)
    {
        var eventId = ValidationServices.ValidateAndParseGuid(id);
        _eventService.Remove(eventId);
    }

    [HttpPut("{id}")]
    [AuthorizationFilter]
    public void UpdateEvent(CreateEventRequest request, string id)
    {
        ArgumentNullException.ThrowIfNull(request);
        var eventId = ValidationServices.ValidateAndParseGuid(id);
        var args = request.ToArgs();
        _eventService.Update(args, eventId);
    }
}
