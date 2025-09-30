using VirtualPark.BusinessLogic.Rankings.Entity;
using VirtualPark.BusinessLogic.Rankings.Models;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Rankings.Service;

public sealed class RankingService(IRepository<Ranking> rankingRepository, IReadOnlyRepository<User> userReadOnlyRepository)
{
    private readonly IRepository<Ranking> _rankingRepository = rankingRepository;
    private readonly IReadOnlyRepository<User> _userReadOnlyRepository = userReadOnlyRepository;
    
}
