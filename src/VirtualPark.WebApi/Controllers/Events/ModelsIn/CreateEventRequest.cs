namespace VirtualPark.WebApi.Controllers.Events.ModelsIn;

public sealed class CreateEventRequest
{
    public string? Capacity { get; set; }
    public string? Date { get; init; }
    public string? Name { get; init; }
    public string? Cost { get; set; }
}
