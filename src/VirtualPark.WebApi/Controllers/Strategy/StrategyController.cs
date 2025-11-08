using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Strategy.Models;
using VirtualPark.BusinessLogic.Strategy.Services;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Strategy.ModelsIn;
using VirtualPark.WebApi.Controllers.Strategy.ModelsOut;
using VirtualPark.WebApi.Filters.Authenticator;
using VirtualPark.WebApi.Filters.Authorization;

namespace VirtualPark.WebApi.Controllers.Strategy;

[ApiController]
[AuthenticationFilter]
[Route("strategies")]
public class StrategyController(IStrategyService strategyService) : ControllerBase
{
    private readonly IStrategyService _strategyService = strategyService;

    [HttpPost]
    [AuthorizationFilter]
    public CreateActiveStrategyResponse CreateActiveStrategy(CreateActiveStrategyRequest request)
    {
        ActiveStrategyArgs strategyArgs = request.ToArgs();

        Guid strategyId = _strategyService.Create(strategyArgs);

        return new CreateActiveStrategyResponse(strategyId.ToString());
    }

    [HttpGet("{date}")]
    [AuthorizationFilter]
    public ActionResult<GetActiveStrategyResponse> GetActiveStrategy(string date)
    {
        var dateOnly = ValidationServices.ValidateDateOnly(date);

        var strategy = _strategyService.Get(dateOnly);

        return new GetActiveStrategyResponse(
            key: strategy.StrategyKey,
            date: strategy.Date.ToString("yyyy-MM-dd"));
    }

    [HttpGet]
    [AuthorizationFilter]
    public ActionResult<IEnumerable<GetActiveStrategyResponse>> GetActiveStrategies()
    {
        var list = _strategyService.GetAll()
            .Select(a => new GetActiveStrategyResponse(
                key: a.StrategyKey,
                date: a.Date?.ToString("yyyy-MM-dd") ?? string.Empty))
            .ToList();

        return list;
    }

    [HttpDelete("{date}")]
    [AuthorizationFilter]
    public IActionResult DeleteStrategy(string date)
    {
        var dateOnly = ValidationServices.ValidateDateOnly(date);
        _strategyService.Remove(dateOnly);
        return NoContent();
    }

    [HttpPut("{date}")]
    [AuthorizationFilter]
    public IActionResult UpdateStrategy([FromBody] UpdateActiveStrategyRequest request, string date)
    {
        var dateOnly = ValidationServices.ValidateDateOnly(date);
        var args = request.ToArgs(dateOnly);
        _strategyService.Update(args, dateOnly);
        return NoContent();
    }
}
