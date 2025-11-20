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
        var rewards = _rewardRepository.GetAll(r => r.QuantityAvailable > 0);

        if(rewards.Count == 0)
        {
            throw new InvalidOperationException("There are no active rewards.");
        }

        return rewards;
    }

    public List<Reward> GetDeleted()
    {
        return _rewardRepository.GetAll(r => r.QuantityAvailable == 0);
    }

    public void Restore(Guid id, int quantity)
    {
        Reward reward = _rewardRepository.Get(rw => rw.Id == id)
                        ?? throw new InvalidOperationException($"Reward with id {id} not found.");

        reward.QuantityAvailable = quantity;
        _rewardRepository.Update(reward);
    }

    public void Remove(Guid id)
    {
        Reward reward = _rewardRepository.Get(rw => rw.Id == id)
                        ?? throw new InvalidOperationException($"Reward with id {id} not found.");

        reward.QuantityAvailable = 0;
        _rewardRepository.Update(reward);
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
