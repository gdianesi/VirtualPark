using VirtualPark.BusinessLogic.Events.Entity;

namespace VirtualPark.WebApi.Controllers.Events.ModelsOut;

public sealed class GetEventResponse(Event ev)
{
    public string Id { get; } = ev.Id.ToString();
    public string Name { get; } = ev.Name;
    public string Date { get; } = ev.Date.ToString("yyyy-MM-dd");
    public string Capacity { get; } = ev.Capacity.ToString();
    public string Cost { get; } = ev.Cost.ToString();
    public List<string> Attractions { get; } = ev.Attractions.Select(a => a.Id.ToString()).ToList();
    public string TicketsSold { get; } = ev.Tickets?.Count.ToString() ?? "0";
}
