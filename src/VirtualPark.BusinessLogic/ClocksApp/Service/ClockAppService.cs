namespace VirtualPark.BusinessLogic.ClocksApp.Service;

public class ClockAppService : IClockAppService
{
    public DateTime Now()
    {
        throw new NotImplementedException();
    }

    public int CalculateDifferenceInMinutes(DateTime systemDateTime)
    {
        return (int)Math.Round((systemDateTime - DateTime.Now).TotalMinutes);
    }
}
