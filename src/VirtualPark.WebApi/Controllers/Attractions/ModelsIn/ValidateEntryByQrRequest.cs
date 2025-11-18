namespace VirtualPark.WebApi.Controllers.Attractions.ModelsIn;

public sealed record ValidateEntryByQrRequest
{
    public string? QrId { get; init; }
}
