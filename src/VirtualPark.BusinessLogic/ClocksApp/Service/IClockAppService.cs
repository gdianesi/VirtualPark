using VirtualPark.BusinessLogic.ClocksApp.Entity;
using VirtualPark.BusinessLogic.ClocksApp.Models;

namespace VirtualPark.BusinessLogic.ClocksApp.Service;

public interface IClockAppService
{
    public ClockApp Get();
    public void Update(ClockAppArgs args);
    DateTime Now();
}
