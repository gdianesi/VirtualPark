using System.Linq.Expressions;
using VirtualPark.BusinessLogic.ClocksApp.Entity;
using VirtualPark.BusinessLogic.ClocksApp.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.ClocksApp.Service;

public sealed class ClockAppService(IRepository<ClockApp> clockAppRepository) : IClockAppService
{
    private readonly IRepository<ClockApp> _clockAppRepository = clockAppRepository;
    
    public DateTime Now()
    {
        throw new NotImplementedException();
    }
}
