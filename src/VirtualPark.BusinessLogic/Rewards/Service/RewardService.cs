using VirtualPark.BusinessLogic.Rewards.Entity;
using VirtualPark.BusinessLogic.Rewards.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Rewards.Service;

public sealed class RewardService(IRepository<Reward> rewardRepository) : IRewardService
{
    private readonly IRepository<Reward> _rewardRepository = rewardRepository;

    public Guid Create(RewardArgs args)
    {
        Reward reward = MapToEntity(args);

        _rewardRepository.Add(reward);

        return reward.Id;
    }

    public Reward Get(Guid id)
    {
        Reward reward = _rewardRepository.Get(rw => rw.Id == id)
                        ?? throw new InvalidOperationException($"Reward with id {id} not found.");

        return reward;
    }

    public Reward? Get(RewardArgs args)
    {
        throw new NotImplementedException();
    }

    public List<Reward> GetAll()
    {
        throw new NotImplementedException();
    }

    public void Remove(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Update(RewardArgs args, Guid incidenceId)
    {
        throw new NotImplementedException();
    }

    private static Reward MapToEntity(RewardArgs args)
    {
        return new Reward
        {
            Cost = args.Cost,
            Description = args.Description,
            Name = args.Name,
            QuantityAvailable = args.QuantityAvailable,
            RequiredMembershipLevel = args.RequiredMembershipLevel
        };
    }
}
