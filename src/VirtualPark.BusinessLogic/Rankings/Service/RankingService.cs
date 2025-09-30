using VirtualPark.BusinessLogic.Rankings.Entity;
using VirtualPark.BusinessLogic.Rankings.Models;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Rankings.Service;

public sealed class RankingService(IRepository<Ranking> rankingRepository, IReadOnlyRepository<User> userReadOnlyRepository)
{
    private readonly IRepository<Ranking> _rankingRepository = rankingRepository;
    private readonly IReadOnlyRepository<User> _userReadOnlyRepository = userReadOnlyRepository;

    public Ranking MapToEntity(RankingArgs rankingArgs)
    {
        return new Ranking
        {
            Date = rankingArgs.Date,
            Entries = GuidToUser(rankingArgs.Entries),
            Period = rankingArgs.Period
        };
    }

    public List<User> GuidToUser(List<Guid> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        return entries.Select(guid => _userReadOnlyRepository.Get(u => u.Id == guid) ?? throw new KeyNotFoundException($"User with id {guid} does not exist")).ToList();
    }
}
