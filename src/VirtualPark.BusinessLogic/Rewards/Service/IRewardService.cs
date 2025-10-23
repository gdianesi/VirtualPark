using VirtualPark.BusinessLogic.Rewards.Entity;
using VirtualPark.BusinessLogic.Rewards.Models;

namespace VirtualPark.BusinessLogic.Rewards.Service;

public interface IRewardService
{
    public Guid Create(RewardArgs args);
    public Reward? Get(Guid id);
    public List<Reward> GetAll();
    public void Remove(Guid id);
    public void Update(RewardArgs args, Guid incidenceId);
}
