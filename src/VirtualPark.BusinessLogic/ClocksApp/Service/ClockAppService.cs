using System.Linq.Expressions;
using VirtualPark.BusinessLogic.ClocksApp.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.ClocksApp.Service;

public class ClockAppService (IRepository<ClockApp> clockAppRepository) : IClockAppService
{
}
