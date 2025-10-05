using VirtualPark.BusinessLogic.Rankings.Entity;
using VirtualPark.BusinessLogic.Rankings.Models;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Rankings.Service;

public sealed class RankingService(IRepository<Ranking> rankingRepository, IReadOnlyRepository<User> userReadOnlyRepository)
{
    private readonly IRepository<Ranking> _rankingRepository = rankingRepository;
    private readonly IReadOnlyRepository<User> _userReadOnlyRepository = userReadOnlyRepository;

    public Guid Create(RankingArgs rankingArgs)
    {
        var ranking = MapToEntity(rankingArgs);

        _rankingRepository.Add(ranking);

        return ranking.Id;
    }

    public List<Ranking> GetAll()
    {
        return _rankingRepository.GetAll();
    }

    public Ranking Get(Guid rankingId)
    {
        var raking = _rankingRepository.Get(r => r.Id == rankingId);

        if(raking == null)
        {
            throw new InvalidOperationException("Raking don't exist");
        }

        return raking;
    }

    public void Update(RankingArgs rankingArgs, Guid id)
    {
        var ranking = Get(id);
        ApplyArgsToEntity(ranking, rankingArgs);
        _rankingRepository.Update(ranking);
    }

    public void Remove(Guid id)
    {
        var ranking = Get(id);
        _rankingRepository.Remove(ranking!);
    }

    public Ranking MapToEntity(RankingArgs rankingArgs)
    {
        return new Ranking
        {
            Date = rankingArgs.Date,
            Entries = GuidToUser(rankingArgs.Entries),
            Period = rankingArgs.Period
        };
    }

    public void ApplyArgsToEntity(Ranking entity, RankingArgs args)
    {
        entity.Date = args.Date;
        entity.Entries = GuidToUser(args.Entries);
        entity.Period = args.Period;
    }

    public List<User> GuidToUser(List<Guid> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        return entries.Select(guid => _userReadOnlyRepository.Get(u => u.Id == guid) ?? throw new KeyNotFoundException($"User with id {guid} does not exist")).ToList();
    }
}
