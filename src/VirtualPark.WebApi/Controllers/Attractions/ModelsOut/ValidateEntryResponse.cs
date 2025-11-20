namespace VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

public sealed record ValidateEntryResponse
{
    public bool IsValid { get; init; }
}
