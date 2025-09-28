using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.TypeIncidences.Models;

public sealed class TypeIncidenceArgs(string type)
{
    public string Type { get; set; } = ValidationServices.ValidateNullOrEmpty(type);
}
