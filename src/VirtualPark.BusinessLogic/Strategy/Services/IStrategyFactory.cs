namespace VirtualPark.BusinessLogic.Strategy.Services;

public interface IStrategyFactory
{
    IStrategy Create(string strategyKey);
}
