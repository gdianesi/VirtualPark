using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.Tickets.Models;
using VirtualPark.BusinessLogic.Tickets.Service;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Tickets.ModelsIn;
using VirtualPark.WebApi.Controllers.Tickets.ModelsOut;
using VirtualPark.WebApi.Filters.Authenticator;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.Tickets;

[ApiController]
[AuthenticationFilter]
public sealed class TicketController(ITicketService ticketService) : ControllerBase
{
    private readonly ITicketService _ticketService = ticketService;

    [HttpGet("/tickets/{id}")]
    [AuthorizationFilter]
    public GetTicketResponse GetTicketById(string id)
    {
        var ticketId = ValidationServices.ValidateAndParseGuid(id);
        var ticket = _ticketService.Get(ticketId)!;
        return MapToResponse(ticket);
    }

    private static GetTicketResponse MapToResponse(Ticket ticket)
    {
        return new GetTicketResponse(
            id: ticket.Id.ToString(),
            type: ticket.Type.ToString(),
            date: ticket.Date.ToString("yyyy-MM-dd"),
            eventId: ticket.EventId.ToString(),
            qrId: ticket.QrId.ToString(),
            visitorId: ticket.VisitorProfileId.ToString());
    }

    [HttpPost("/tickets")]
    [AuthorizationFilter]
    public CreateTicketResponse CreateTicket(CreateTicketRequest request)
    {
        TicketArgs args = request.ToArgs();
        Guid ticketId = _ticketService.Create(args);
        return new CreateTicketResponse(ticketId.ToString());
    }

    [HttpGet("/tickets")]
    [AuthorizationFilter]
    public List<GetTicketResponse> GetAllTickets()
    {
        return _ticketService
            .GetAll()
            .Select(MapToResponse)
            .ToList();
    }

    [HttpDelete("/tickets/{id}")]
    [AuthorizationFilter]
    public void DeleteTicket(string id)
    {
        var ticketId = ValidationServices.ValidateAndParseGuid(id);
        _ticketService.Remove(ticketId);
    }

    [HttpGet("/tickets/visitor/{visitorId}")]
    [AuthorizationFilter]
    public List<GetTicketResponse> GetTicketsByVisitor(string visitorId)
    {
        var id = ValidationServices.ValidateAndParseGuid(visitorId);

        var tickets = _ticketService.GetTicketsByVisitor(id);

        return tickets.Select(MapToResponse).ToList();
    }
}
