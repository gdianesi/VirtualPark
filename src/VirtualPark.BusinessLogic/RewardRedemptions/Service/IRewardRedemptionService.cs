using VirtualPark.BusinessLogic.RewardRedemptions.Entity;
using VirtualPark.BusinessLogic.RewardRedemptions.Models;

namespace VirtualPark.BusinessLogic.RewardRedemptions.Service;

public interface IRewardRedemptionService
{
    Guid RedeemReward(RewardRedemptionArgs args);
    RewardRedemption Get(Guid id);
    List<RewardRedemption> GetAll();
    List<RewardRedemption> GetByVisitor(Guid visitorId);
}
