using VirtualPark.BusinessLogic.Strategy.Entity;
using VirtualPark.BusinessLogic.Strategy.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public sealed class ActiveStrategyService(IRepository<ActiveStrategy> activeStrategyRepository )
{
    private readonly IRepository<ActiveStrategy> _activeStrategyRepository = activeStrategyRepository;
    
}
