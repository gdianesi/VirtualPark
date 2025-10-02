using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.ClocksApp.Models;

public sealed class ClockAppArgs(string date)
{
    public DateTime SystemDateTime { get; init; } = ValidationServices.ValidateDateTime(date);
}
