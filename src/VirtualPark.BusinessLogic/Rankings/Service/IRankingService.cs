using VirtualPark.BusinessLogic.Rankings.Entity;
using VirtualPark.BusinessLogic.Rankings.Models;

namespace VirtualPark.BusinessLogic.Rankings.Service;

public interface IRankingService
{
    public Guid Create(RankingArgs args);
    public Ranking? Get(Guid id);
    Ranking? Get(RankingArgs args);
    public List<Ranking> GetAll();
    public void Remove(Guid id);
    public void Update(RankingArgs args, Guid incidenceId);
}
