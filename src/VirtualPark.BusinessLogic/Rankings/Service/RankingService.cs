using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Rankings.Entity;
using VirtualPark.BusinessLogic.Rankings.Models;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Rankings.Service;

public sealed class RankingService(IRepository<Ranking> rankingRepository, IReadOnlyRepository<User> userReadOnlyRepository,
    IReadOnlyRepository<VisitRegistration> visitRegistrationsReadOnlyRepository) : IRankingService
{
    private readonly IRepository<Ranking> _rankingRepository = rankingRepository;
    private readonly IReadOnlyRepository<User> _userReadOnlyRepository = userReadOnlyRepository;
    private readonly IReadOnlyRepository<VisitRegistration> _visitRegistrationsReadOnlyRepository = visitRegistrationsReadOnlyRepository;

    public Guid Create(RankingArgs rankingArgs)
    {
        var ranking = MapToEntity(rankingArgs);

        _rankingRepository.Add(ranking);

        return ranking.Id;
    }

    public Ranking? Get(RankingArgs args)
    {
        var topUsers = CalculateRanking(args.Date);

        var ranking = _rankingRepository.Get(
            r => r.Date.Date == args.Date.Date &&
                 r.Period == args.Period,
            q => q.Include(r => r.Entries));

        if(ranking is null)
        {
            ranking = MapToEntity(args);
            ranking.Entries = topUsers;
            _rankingRepository.Add(ranking);
        }
        else
        {
            ranking.Entries = topUsers;
            _rankingRepository.Update(ranking);
        }

        return ranking;
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
            Period = rankingArgs.Period
        };
    }

    public void ApplyArgsToEntity(Ranking entity, RankingArgs args)
    {
        entity.Date = args.Date;
        entity.Period = args.Period;
    }

    public List<User> GuidToUser(List<Guid> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        return entries.Select(guid => _userReadOnlyRepository.Get(u => u.Id == guid) ?? throw new KeyNotFoundException($"User with id {guid} does not exist")).ToList();
    }

    private List<User> CalculateRanking(DateTime date)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);

        var visitsOfDay = _visitRegistrationsReadOnlyRepository.GetAll()
            .Where(v => v.Date >= startOfDay && v.Date < endOfDay)
            .ToList();

        var visitorScores = visitsOfDay
            .GroupBy(v => v.VisitorId)
            .Select(g => new
            {
                VisitorId = g.Key,
                TotalScore = g.Sum(v => v.DailyScore)
            })
            .OrderByDescending(x => x.TotalScore)
            .Take(10)
            .ToList();

        var top10Users = visitorScores
            .Select(vs =>
            {
                var user = _userReadOnlyRepository.Get(
                    u => u.VisitorProfileId == vs.VisitorId,
                    q => q.Include(u => u.VisitorProfile));

                if(user?.VisitorProfile != null)
                {
                    user.VisitorProfile.Score = vs.TotalScore;
                }

                return user;
            })
            .Where(u => u != null)
            .ToList()!;

        return top10Users;
    }
}
