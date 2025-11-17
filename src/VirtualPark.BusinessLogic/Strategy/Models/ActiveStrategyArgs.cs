using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Strategy.Models;

public class ActiveStrategyArgs(string strategyKey, string date)
{
    public string StrategyKey { get; } = ValidationServices.ValidateNullOrEmpty(strategyKey);
    public DateOnly Date { get; } = ValidationServices.ValidateDateOnly(date);
}
