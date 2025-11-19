using VirtualPark.BusinessLogic.Strategy.Models;

namespace VirtualPark.WebApi.Controllers.Strategy.ModelsOut;

public class GetActiveStrategyResponse(ActiveStrategyArgs? strategy)
{
    public string Key { get; } = strategy.StrategyKey;
    public string Date { get; } = strategy.Date.ToString("yyyy-MM-dd");
}
