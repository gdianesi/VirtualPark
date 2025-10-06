using VirtualPark.BusinessLogic.Events.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.Events.ModelsIn;

public sealed class CreateEventRequest
{
    public List<string>? AttractionsIds { get; init; }
    public string? Capacity { get; init; }
    public string? Cost { get; init; }
    public string? Date { get; init; }
    public string? Name { get; init; }

    public EventsArgs ToArgs()
    {
        var validatedName = ValidationServices.ValidateNullOrEmpty(Name);
        var validatedDate = ValidationServices.ValidateNullOrEmpty(Date);
        var validatedCapacity = ValidationServices.ValidatePositive(int.Parse(ValidationServices.ValidateNullOrEmpty(Capacity)));
        var validatedCost = ValidationServices.ValidatePositive(int.Parse(ValidationServices.ValidateNullOrEmpty(Cost)));
        var validatedAttractions = AttractionsIds ?? throw new ArgumentException("Attractions list cannot be null");

        return new EventsArgs(validatedName, validatedDate, validatedCapacity, validatedCost, validatedAttractions);
    }
}
