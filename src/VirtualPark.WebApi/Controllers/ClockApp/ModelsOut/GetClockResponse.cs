namespace VirtualPark.WebApi.Controllers.ClockApp.ModelsOut;

public class GetClockResponse(string dateSystem)
{
    public string DateSystem { get; } = dateSystem;
}
