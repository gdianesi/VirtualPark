namespace VirtualPark.WebApi.Controllers.Events.ModelsOut;

public class GetEventResponse(string id)
{
    public string Id { get; } = id;
}
