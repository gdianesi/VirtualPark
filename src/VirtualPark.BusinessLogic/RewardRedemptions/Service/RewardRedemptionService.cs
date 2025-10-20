using VirtualPark.BusinessLogic.RewardRedemptions.Entity;
using VirtualPark.BusinessLogic.RewardRedemptions.Models;
using VirtualPark.BusinessLogic.Rewards.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.RewardRedemptions.Service;

public sealed class RewardRedemptionService(
    IRepository<Reward> rewardRepository,
    IRepository<RewardRedemption> redemptionRepository,
    IRepository<VisitorProfile> visitorRepository)
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

        ValidatePoints(visitor, reward);

        reward.QuantityAvailable--;
        visitor.Score -= args.PointsSpent;

        var redemption = new RewardRedemption
        {
            RewardId = args.RewardId,
            VisitorId = args.VisitorId,
            Date = args.Date,
            PointsSpent = args.PointsSpent
        };

        _redemptionRepository.Add(redemption);
        _rewardRepository.Update(reward);
        _visitorRepository.Update(visitor);

        return redemption.Id;
    }

    private static void ValidatePoints(VisitorProfile visitor, Reward reward)
    {
        if (visitor.Score < reward.Cost)
        {
            throw new InvalidOperationException("Visitor does not have enough points to redeem this reward.");
        }
    }
}
