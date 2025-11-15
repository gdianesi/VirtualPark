using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Strategy.Models;

public class StrategyArgs(string strategyKey)
{
    public string Key { get; set; } = ValidationServices.ValidateNullOrEmpty(strategyKey);
}
