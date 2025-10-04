using System.Linq.Expressions;
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
        var clock = _clockAppRepository.GetAll().FirstOrDefault();
        return clock ?? new ClockApp();
    }

    public DateTime Now()
    {
        var clock = Get();
        return clock.DateSystem;
    }

    private ClockApp MapToEntity(ClockAppArgs clockAppArgs)
    {
        ClockApp clockApp = new ClockApp { DateSystem = clockAppArgs.SystemDateTime };
        return clockApp;
    }
}
