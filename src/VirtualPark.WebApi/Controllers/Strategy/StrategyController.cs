using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Strategy.Models;
using VirtualPark.BusinessLogic.Strategy.Services;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Strategy.ModelsIn;
using VirtualPark.WebApi.Controllers.Strategy.ModelsOut;

namespace VirtualPark.WebApi.Controllers.Strategy;

[Route("strategies")]
public class StrategyController(IStrategyService strategyService) : ControllerBase
{
    private readonly IStrategyService _strategyService = strategyService;

    [HttpPost]
    public CreateActiveStrategyResponse CreateActiveStrategy(CreateActiveStrategyRequest request)
    {
        ActiveStrategyArgs strategyArgs = request.ToArgs();

        Guid strategyId = _strategyService.Create(strategyArgs);

        return new CreateActiveStrategyResponse(strategyId.ToString());
    }

    [HttpGet("strategies/{date}")]
    public GetActiveStrategyResponse GetActiveStrategy(string date)
    {
        var dateOnly = ValidationServices.ValidateDateOnly(date);

        var strategy = _strategyService.Get(dateOnly);

        return new GetActiveStrategyResponse(
            key: strategy.StrategyKey,
            date: strategy.Date.ToString("yyyy-MM-dd")
        );
    }

    [HttpGet]
    public List<GetActiveStrategyResponse> GetActiveStrategies()
    {
        var strategies = _strategyService.GetAll()
            .Select(a => new GetActiveStrategyResponse(
                a.StrategyKey, a.Date.ToString("yyyy-MM-dd")))
                .ToList();

        return strategies;
    }

    [HttpGet("strategies/{date}")]
    public void DeletStrategy(string date)
    {
        var dateOnly = ValidationServices.ValidateDateOnly(date);
        _strategyService.Remove(dateOnly);
    }

    [HttpPut("strategies/{date}")]
    public void UpdateStrategy(CreateActiveStrategyRequest request)
    {
        var dateOnly = ValidationServices.ValidateDateOnly(date);

        ActiveStrategyArgs strategyArgs = request.ToArgs();

        _strategyService.Update(strategyArgs, dateOnly);
    }
}
