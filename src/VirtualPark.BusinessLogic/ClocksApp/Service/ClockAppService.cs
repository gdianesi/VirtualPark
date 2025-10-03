using VirtualPark.BusinessLogic.ClocksApp.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.ClocksApp.Service;

public class ClockAppService : IClockAppService
{
    private readonly IRepository<ClockApp> _clockAppRepository;
    private readonly ClockApp _clockApp;

    public ClockAppService(IRepository<ClockApp> clockAppRepository)
    {
        _clockAppRepository = clockAppRepository;

        _clockApp = _clockAppRepository.GetAll().FirstOrDefault();
        if (_clockApp is null)
        {
            _clockApp = new ClockApp();
            _clockAppRepository.Add(_clockApp);
        }
    }

    public int CalculateDifferenceInMinutes(DateTime systemDateTime) => (int)Math.Round((systemDateTime - DateTime.Now).TotalMinutes);

    public DateTime Now() => DateTime.Now.AddMinutes(_clockApp.OffsetMinutes);
}
