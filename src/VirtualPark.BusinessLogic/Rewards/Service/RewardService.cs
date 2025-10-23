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

    public Reward? Get(Guid id)
    {
        Reward reward = _rewardRepository.Get(rw => rw.Id == id)
                        ?? throw new InvalidOperationException($"Reward with id {id} not found.");

        return reward;
    }

    public List<Reward> GetAll()
    {
        List<Reward>? rewards = _rewardRepository.GetAll();

        if(rewards == null || rewards.Count == 0)
        {
            throw new InvalidOperationException("There are no rewards registered.");
        }

        return rewards;
    }

    public void Remove(Guid id)
    {
        Reward reward = _rewardRepository.Get(rw => rw.Id == id)
                        ?? throw new InvalidOperationException($"Reward with id {id} not found.");

        _rewardRepository.Remove(reward);
    }

    public void Update(RewardArgs args, Guid id)
    {
        Reward reward = _rewardRepository.Get(rw => rw.Id == id)
                        ?? throw new InvalidOperationException($"Reward with id {id} not found.");

        ApplyArgsToEntity(reward, args);

        _rewardRepository.Update(reward);
    }

    private static void ApplyArgsToEntity(Reward entity, RewardArgs args)
    {
        entity.Name = args.Name;
        entity.Description = args.Description;
        entity.Cost = args.Cost;
        entity.QuantityAvailable = args.QuantityAvailable;
        entity.RequiredMembershipLevel = args.RequiredMembershipLevel;
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
