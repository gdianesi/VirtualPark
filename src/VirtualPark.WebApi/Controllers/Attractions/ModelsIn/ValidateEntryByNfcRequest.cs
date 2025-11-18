namespace VirtualPark.WebApi.Controllers.Attractions.ModelsIn;

public sealed record ValidateEntryByNfcRequest
{
    public string? VisitorId { get; init; }
}
