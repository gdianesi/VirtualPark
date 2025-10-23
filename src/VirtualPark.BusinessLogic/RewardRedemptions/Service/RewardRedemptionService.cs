using VirtualPark.BusinessLogic.RewardRedemptions.Entity;
using VirtualPark.BusinessLogic.RewardRedemptions.Models;
using VirtualPark.BusinessLogic.Rewards.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.RewardRedemptions.Service;

public sealed class RewardRedemptionService(
    IRepository<Reward> rewardRepository,
    IRepository<RewardRedemption> redemptionRepository,
    IRepository<VisitorProfile> visitorRepository) : IRewardRedemptionService
{
    private readonly IRepository<Reward> _rewardRepository = rewardRepository;
    private readonly IRepository<RewardRedemption> _redemptionRepository = redemptionRepository;
    private readonly IRepository<VisitorProfile> _visitorRepository = visitorRepository;

    public Guid RedeemReward(RewardRedemptionArgs args)
    {
        Reward reward = _rewardRepository.Get(rw => rw.Id == args.RewardId)
                        ?? throw new InvalidOperationException($"Reward with id {args.RewardId} not found.");

        VisitorProfile visitor = _visitorRepository.Get(v => v.Id == args.VisitorId)
                                 ?? throw new InvalidOperationException($"Visitor with id {args.VisitorId} not found.");

        ValidateAvailability(reward);
        ValidatePoints(visitor, reward);

        reward.QuantityAvailable--;
        visitor.Score -= args.PointsSpent;

        RewardRedemption redemption = MapToEntity(args);

        _redemptionRepository.Add(redemption);
        _rewardRepository.Update(reward);
        _visitorRepository.Update(visitor);

        return redemption.Id;
    }

    private static RewardRedemption MapToEntity(RewardRedemptionArgs args)
    {
        var redemption = new RewardRedemption
        {
            RewardId = args.RewardId,
            VisitorId = args.VisitorId,
            Date = args.Date,
            PointsSpent = args.PointsSpent
        };
        return redemption;
    }

    public RewardRedemption Get(Guid id)
    {
        RewardRedemption redemption = _redemptionRepository.Get(r => r.Id == id)
                                      ?? throw new InvalidOperationException($"Reward redemption with id {id} not found.");

        return redemption;
    }

    public List<RewardRedemption> GetAll()
    {
        List<RewardRedemption>? redemptions = _redemptionRepository.GetAll();

        if(redemptions == null || redemptions.Count == 0)
        {
            throw new InvalidOperationException("There are no reward redemptions registered.");
        }

        return redemptions;
    }

    public List<RewardRedemption> GetByVisitor(Guid visitorId)
    {
        List<RewardRedemption> redemptions = _redemptionRepository.GetAll(r => r.VisitorId == visitorId);

        if(redemptions == null || redemptions.Count == 0)
        {
            throw new InvalidOperationException($"Visitor with id {visitorId} has no redemptions.");
        }

        return redemptions;
    }

    private static void ValidateAvailability(Reward reward)
    {
        if(reward.QuantityAvailable <= 0)
        {
            throw new InvalidOperationException("Reward is not available.");
        }
    }

    private static void ValidatePoints(VisitorProfile visitor, Reward reward)
    {
        if(visitor.Score < reward.Cost)
        {
            throw new InvalidOperationException("Visitor does not have enough points to redeem this reward.");
        }
    }
}
