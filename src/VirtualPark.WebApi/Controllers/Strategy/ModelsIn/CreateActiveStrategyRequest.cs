using VirtualPark.BusinessLogic.Strategy.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.Strategy.ModelsIn;

public class CreateActiveStrategyRequest
{
    public string? StrategyKey { get; init; }
    public string? Date { get; init; }

    public ActiveStrategyArgs ToArgs()
    {
        return new ActiveStrategyArgs(
            ValidationServices.ValidateNullOrEmpty(StrategyKey),
            ValidationServices.ValidateNullOrEmpty(Date));
    }
}
