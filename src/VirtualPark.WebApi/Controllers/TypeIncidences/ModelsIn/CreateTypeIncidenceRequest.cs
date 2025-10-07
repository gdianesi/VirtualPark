using VirtualPark.BusinessLogic.TypeIncidences.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.TypeIncidences.ModelsIn;

public class CreateTypeIncidenceRequest
{
    public string? Type { get; init; }

    public TypeIncidenceArgs ToArgs()
    {
        return new TypeIncidenceArgs(
            ValidationServices.ValidateNullOrEmpty(Type));
    }
}
