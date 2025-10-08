using VirtualPark.BusinessLogic.Strategy.Models;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public interface IStrategyService
{
    public Guid Create(ActiveStrategyArgs args);
    public ActiveStrategyArgs? Get(DateOnly date);
    public List<ActiveStrategyArgs> GetAll();
    public void Remove(DateOnly id);
    public void Update(ActiveStrategyArgs args, DateOnly date);
}
