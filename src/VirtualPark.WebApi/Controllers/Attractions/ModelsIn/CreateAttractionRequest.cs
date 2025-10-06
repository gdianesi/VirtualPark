using VirtualPark.BusinessLogic.Attractions.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.Attractions.ModelsIn;

public sealed class CreateAttractionRequest
{
    public string? Name { get; init; }
    public string? Type { get; init; }
    public string? MiniumAge { get; init; }
    public string? Capacity { get; init; }
    public string? Description { get; init; }
    public string? Available { get; init; }

    public AttractionArgs ToArgs()
    {
        return new AttractionArgs(
            ValidationServices.ValidateNullOrEmpty(Type),
            ValidationServices.ValidateNullOrEmpty(Name),
            ValidationServices.ValidateNullOrEmpty(MiniumAge),
            ValidationServices.ValidateNullOrEmpty(Capacity),
            ValidationServices.ValidateNullOrEmpty(Description),
            "0",
            ValidationServices.ValidateNullOrEmpty(Available));
    }
}
