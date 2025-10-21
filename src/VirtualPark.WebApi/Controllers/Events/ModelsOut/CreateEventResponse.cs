namespace VirtualPark.WebApi.Controllers.Events.ModelsOut;

public sealed class CreateEventResponse(string id)
{
    public string Id { get; } = id;
}
