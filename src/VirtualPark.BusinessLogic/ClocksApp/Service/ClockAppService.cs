namespace VirtualPark.BusinessLogic.ClocksApp.Service;

public class ClockAppService : IClockAppService
{
    public DateTime Now()
    {
        throw new NotImplementedException();
    }

    public int CalculateDifferenceInMinutes(DateTime systemDateTime)
    {
        var diffMinutes = (int)Math.Round((systemDateTime - DateTime.Now).TotalMinutes);
        return diffMinutes;
    }
}
