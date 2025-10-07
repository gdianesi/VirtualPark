using VirtualPark.BusinessLogic.ClocksApp.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.ClockApp.ModelsIn;

public class UpdateClockRequest
{
    public string? DateSystem { get; set; }

    public ClockAppArgs ToArgs()
    {
        var validatedDateSystem = ValidationServices.ValidateNullOrEmpty(DateSystem);
        return new ClockAppArgs(validatedDateSystem);
    }
}
