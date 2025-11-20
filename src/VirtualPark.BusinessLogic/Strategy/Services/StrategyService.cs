using VirtualPark.BusinessLogic.Strategy.Entity;
using VirtualPark.BusinessLogic.Strategy.Models;
using VirtualPark.Reflection;
using VirtualPark.ReflectionAbstractions;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public sealed class ActiveStrategyService(IRepository<ActiveStrategy> activeStrategyRepository, IStrategyFactory strategyFactory, ILoadAssembly<IStrategy> loadAssembly) : IStrategyService
{
    private readonly IRepository<ActiveStrategy> _activeStrategyRepository = activeStrategyRepository;
    private readonly IStrategyFactory _strategyFactory = strategyFactory;
    private readonly ILoadAssembly<IStrategy> _loadAssembly = loadAssembly;

    public Guid Create(ActiveStrategyArgs args)
    {
        try
        {
            _strategyFactory.Create(args.StrategyKey);
        }
        catch(KeyNotFoundException)
        {
            throw new ArgumentException($"The strategy '{args.StrategyKey}' is not valid.");
        }

        var existing = _activeStrategyRepository.Get(a => a.Date == args.Date);
        if(existing is null)
        {
            var entity = MapToEntity(args);
            _activeStrategyRepository.Add(entity);
            return entity.Id;
        }

        existing.StrategyKey = args.StrategyKey;
        _activeStrategyRepository.Update(existing);
        return existing.Id;
    }

    public ActiveStrategyArgs? Get(DateOnly date)
    {
        var entity = _activeStrategyRepository.Get(a => a.Date == date);
        return entity is null ? null : MapToArgs(entity);
    }

    public List<ActiveStrategyArgs> GetAll()
    {
        var entities = _activeStrategyRepository.GetAll();
        return entities.Select(MapToArgs).ToList();
    }

    public List<StrategyArgs> GetAllStrategies()
    {
        var strategies = _loadAssembly.GetImplementationKeys();
        strategies.AddRange(["Attraction", "Combo", "Event"]);

        return strategies
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(s => new StrategyArgs(s!))
            .ToList();
    }

    public void Update(ActiveStrategyArgs args, DateOnly date)
    {
        var entity = _activeStrategyRepository.Get(a => a.Date == date)
            ?? throw new InvalidOperationException($"ActiveStrategy with date {date} not found.");

        entity.StrategyKey = args.StrategyKey;

        _activeStrategyRepository.Update(entity);
    }

    public void Remove(DateOnly date)
    {
        var entity = _activeStrategyRepository.Get(a => a.Date == date)
            ?? throw new InvalidOperationException($"ActiveStrategy with date {date} not found.");

        _activeStrategyRepository.Remove(entity);
    }

    private static ActiveStrategy MapToEntity(ActiveStrategyArgs args) => new()
    {
        StrategyKey = args.StrategyKey,
        Date = args.Date
    };

    private static ActiveStrategyArgs MapToArgs(ActiveStrategy entity) => new(
        entity.StrategyKey,
        entity.Date.ToString("yyyy-MM-dd")
    );
}
