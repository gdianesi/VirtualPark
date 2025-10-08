using VirtualPark.BusinessLogic.Strategy.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.Strategy.ModelsIn;

public class UpdateActiveStrategyRequest
{
    public string? Key { get; set; }

    public ActiveStrategyArgs ToArgs(DateOnly date) =>
        new ActiveStrategyArgs(
            ValidationServices.ValidateNullOrEmpty(Key),
            date.ToString("yyyy-MM-dd"));
}
