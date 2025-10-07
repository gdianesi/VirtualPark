using VirtualPark.BusinessLogic.Strategy.Entity;
using VirtualPark.BusinessLogic.Strategy.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Strategy.Services;

public sealed class ActiveStrategyService(IRepository<ActiveStrategy> activeStrategyRepository )
{
    private readonly IRepository<ActiveStrategy> _activeStrategyRepository = activeStrategyRepository;

    public Guid Create(ActiveStrategyArgs args)
    {
        var existing = _activeStrategyRepository.Get(a => a.Date == args.Date);

        if (existing is null)
        {
            var entity = MapToEntity(args);
            _activeStrategyRepository.Add(entity);
            return entity.Id;
        }
        else
        {
            existing.StrategyKey = args.StrategyKey;
            _activeStrategyRepository.Update(existing);
            return existing.Id;
        }
    }

    public ActiveStrategy? Get(DateOnly date)
        => _activeStrategyRepository.Get(a => a.Date == date);

    public List<ActiveStrategy> GetAll()
        => _activeStrategyRepository.GetAll();

    public void Update(DateOnly date, ActiveStrategyArgs args)
    {
        var entity = Get(date) ?? throw new InvalidOperationException($"ActiveStrategy with date {date} not found.");
        ApplyArgsToEntity(entity, args);
        _activeStrategyRepository.Update(entity);
    }

    public void Remove(DateOnly date)
    {
        var entity = Get(date) ?? throw new InvalidOperationException($"ActiveStrategy with date {date} not found.");
        _activeStrategyRepository.Remove(entity);
    }

    public static void ApplyArgsToEntity(ActiveStrategy entity, ActiveStrategyArgs args)
    {
        entity.StrategyKey = args.StrategyKey;
        entity.Date = args.Date;
    }

    public ActiveStrategy MapToEntity(ActiveStrategyArgs args)
    {
        return new ActiveStrategy
        {
            StrategyKey = args.StrategyKey,
            Date = args.Date
        };
    }
}
