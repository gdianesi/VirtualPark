using VirtualPark.BusinessLogic.ClocksApp.Entity;
using VirtualPark.BusinessLogic.ClocksApp.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.ClocksApp.Service;

public sealed class ClockAppService(IRepository<ClockApp> clockAppRepository) : IClockAppService
{
    private readonly IRepository<ClockApp> _clockAppRepository = clockAppRepository;

    public Guid Create(ClockAppArgs clockAppArgs)
    {
        var clockApp = MapToEntity(clockAppArgs);
        _clockAppRepository.Add(clockApp);
        return clockApp.Id;
    }

    public ClockApp Get()
    {
        var clock = GetFromRepository();
        return clock ?? new ClockApp();
    }

    public void Update(ClockAppArgs clockAppArgs)
    {
        var clockApp = GetFromRepository();

        if(clockApp == null)
        {
            Create(clockAppArgs);
            return;
        }

        clockApp.DateSystem = clockAppArgs.SystemDateTime;
        _clockAppRepository.Update(clockApp);
    }

    public void Remove()
    {
        var clockApp = GetFromRepository();
        if(clockApp != null)
        {
            _clockAppRepository.Remove(clockApp);
        }
    }

    private ClockApp? GetFromRepository() => _clockAppRepository.GetAll().FirstOrDefault();

    public DateTime Now() => Get().DateSystem;

    private ClockApp MapToEntity(ClockAppArgs clockAppArgs)
    {
        var clockApp = new ClockApp { DateSystem = clockAppArgs.SystemDateTime };
        return clockApp;
    }
}
